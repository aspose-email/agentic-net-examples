"""Native operation assertions for the published V4 slice; stdlib only."""
from pathlib import Path
import hashlib
import json
import mailbox
import subprocess
import sys
from email import policy
from email.parser import BytesParser
from email.utils import parsedate_to_datetime
from html.parser import HTMLParser

ROOT = Path(__file__).resolve().parents[1]

class Tags(HTMLParser):
    def __init__(self):
        super().__init__()
        self.tags = []
    def handle_starttag(self, tag, attrs):
        self.tags.append((tag, dict(attrs)))

def require(value, message, checks):
    if not value:
        raise AssertionError(message)
    checks.append(message)

def check(task_id, directory):
    work = Path(directory).resolve()
    manifest = json.loads((ROOT / "manifest.json").read_text())
    row = next(r for r in manifest["examples"] if r["task_id"] == task_id)
    source = ROOT.parent / row["path"]
    checks = []
    require(hashlib.sha256(source.read_bytes()).hexdigest() == row["sha256"], "Published source matches manifest hash", checks)
    for fixture in row["fixtures"]:
        require(hashlib.sha256((ROOT.parent / fixture["path"]).read_bytes()).hexdigest() == fixture["sha256"], "Committed fixture hash: " + fixture["runtime_path"], checks)
    if task_id in {"95552", "95608", "95699"}:
        program = work / "Program.cs"
        original = program.read_bytes()
        try:
            program.write_bytes((ROOT / "validation/native-check.cs.txt").read_bytes())
            build = subprocess.run(["dotnet", "build", "-c", "Release", "--no-restore"], cwd=work, capture_output=True, text=True, timeout=180)
            if build.returncode:
                raise AssertionError("Native inspector build failed: " + build.stdout[-2000:] + build.stderr[-2000:])
            require(True, "Native inspector compiles", checks)
            run = subprocess.run(["dotnet", "run", "-c", "Release", "--no-build", "--", task_id, str(work), str(ROOT / "fixtures" / task_id)], cwd=work, capture_output=True, text=True, timeout=60)
            require(run.returncode == 0, "Native MSG/PST assertions: " + run.stdout + run.stderr, checks)
        finally:
            program.write_bytes(original)
    elif row["category"] == "amp-emails":
        names = {"95676": "amp_styled_accordion.eml", "95679": "amp_viewport_fittext.eml", "95681": "amp_form_subscribe.eml", "95683": "amp_carousel_form.eml", "95685": "amp_header_accordion.eml", "95686": "amp_form_csrf.eml"}
        message = BytesParser(policy=policy.default).parsebytes((work / names[task_id]).read_bytes())
        parts = list(message.walk())
        amp = "".join(p.get_content() for p in parts if p.get_content_type() == "text/x-amp-html")
        html = "".join(p.get_content() for p in parts if p.get_content_type() == "text/html")
        require(bool(amp), "Real AMP MIME part saved", checks)
        parser = Tags(); parser.feed(amp); tags = parser.tags
        if task_id in {"95676", "95685"}:
            n = 3 if task_id == "95676" else 2
            require(any(t == "amp-accordion" for t, a in tags) and sum(t == "section" for t, a in tags) == n, "Accordion has requested section count", checks)
            require(all("Panel " + str(i) in amp for i in range(1, n + 1)), "Panel titles retained", checks)
            if task_id == "95676":
                require(all(s in html for s in ["<style>", "font-family: Arial", "border: 1px solid #ccc"]), "HTML/CSS retained", checks)
            else:
                require("<h1>Welcome to AMP Email</h1>" in html, "HTML header retained", checks)
        elif task_id == "95679":
            require(any(t == "amp-fit-text" for t, a in tags), "FitText component retained", checks)
            h = Tags(); h.feed(html)
            require(any(t == "meta" and a.get("name") == "viewport" and "width=device-width" in a.get("content", "") for t, a in h.tags), "Viewport metadata retained", checks)
        elif task_id in {"95681", "95686"}:
            require(any(t == "form" and a.get("method", "").lower() == "post" for t, a in tags), "Form uses POST", checks)
            label = "Subscribe" if task_id == "95681" else "Submit"
            require(any(t == "input" and a.get("type") == "submit" and a.get("value") == label for t, a in tags), "Submit label retained", checks)
            if task_id == "95686":
                require(any(t == "input" and a.get("type") == "hidden" and a.get("name") == "csrf_token" and a.get("value") == "local-fixture-token-95686" for t, a in tags), "Synthetic local CSRF field retained", checks)
                require(any(t == "input" and a.get("type") == "email" and "required" in a for t, a in tags), "Required email field retained", checks)
        elif task_id == "95683":
            require(sum(t == "amp-img" and a.get("src", "").startswith("https://") for t, a in tags) == 3, "Three active HTTPS carousel images", checks)
            require(0 <= amp.find("<amp-carousel") < amp.find("<form"), "Carousel appears before form", checks)
            require(any(t == "form" and a.get("method", "").lower() == "post" for t, a in tags), "Form below carousel uses POST", checks)
            images = {str(p.get("Content-ID")).strip("<>"): p for p in parts if p.get_content_type() == "image/png"}
            require(set(images) == {"image1", "image2", "image3"}, "All CID fallback attachments retained", checks)
            for cid, part in images.items():
                fixture = ROOT / "fixtures" / task_id / (cid + ".png")
                require(part.get_content_disposition() == "inline" and part.get_payload(decode=True) == fixture.read_bytes() and "cid:" + cid in html, "Inline CID/payload preserved: " + cid, checks)
    elif task_id in {"95562", "95584", "95610"}:
        file = "meeting.ics" if task_id == "95610" else "appointments.ics"
        text = (work / file).read_text()
        require(text.count("BEGIN:VCALENDAR") == text.count("END:VCALENDAR") == 1, "One complete VCALENDAR", checks)
        if task_id == "95610":
            require(text.count("BEGIN:VEVENT") == 1 and "SUMMARY:Planning" in text and "LOCATION:Room 4" in text and "DTSTART:20250501T130000Z" in text and "DTEND:20250501T140000Z" in text, "MSG event fields and UTC times retained", checks)
        else:
            require(text.count("BEGIN:VEVENT") == text.count("END:VEVENT") == 2, "Two complete VEVENT components", checks)
            events = text.split("BEGIN:VEVENT")[1:]
            require("SUMMARY:First meeting" in events[0] and "DTSTART:20270102T100000Z" in events[0] and "SUMMARY:Second meeting" in events[1] and "DTSTART:20270103T100000Z" in events[1], "Both event subjects/start times retained", checks)
            if task_id == "95584":
                require(events[0].count("BEGIN:VALARM") == 1 and "ACTION:DISPLAY" in events[0] and "TRIGGER;RELATED=START:-PT15M" in events[0], "First event display reminder retained", checks)
                require(events[1].count("BEGIN:VALARM") == 1 and "ACTION:AUDIO" in events[1] and "TRIGGER;RELATED=START:-PT10M" in events[1], "Second event audio reminder retained", checks)
    elif task_id in {"95799", "95805", "95837"}:
        if task_id == "95799":
            require((work / "summary.txt").read_text() == "Archive seed(Aspose.Email Evaluation) | 2024-01-02 03:04:05Z\n", "MBOX subject/date metadata report matches input", checks)
        else:
            path = work / "Messages.mbox"
            box = mailbox.mbox(path, create=False)
            try:
                messages = list(box)
            finally:
                box.close()
            require(len(messages) == (1 if task_id == "95805" else 2), "Expected MBOX message count", checks)
            first = messages[0]
            require(str(first["To"]) == "to@example.com" and str(first["Cc"]) == "cc@example.com" and str(first["Bcc"]) == "bcc@example.com", "To/Cc/Bcc retained", checks)
            require("Archive seed" in str(first["Subject"]) and parsedate_to_datetime(first["Date"]).strftime("%Y-%m-%d %H:%M:%S%z") == "2024-01-02 03:04:05+0000", "Original MBOX subject/date retained", checks)
            if task_id == "95837":
                original = (ROOT / "fixtures" / task_id / "Messages.mbox").read_bytes()
                require(path.read_bytes().startswith(original), "Existing MBOX bytes preserved exactly", checks)
                require("Appended message" in str(messages[1]["Subject"]) and str(messages[1]["X-Unsent"]) == "0", "Non-draft appended message retained", checks)
    else:
        raise AssertionError("No native checker for " + task_id)
    return {"task_id": task_id, "source_sha256": row["sha256"], "native_operation_verified": True, "checks": checks}

if __name__ == "__main__":
    result = check(sys.argv[1], sys.argv[2])
    print(json.dumps(result, indent=2))
