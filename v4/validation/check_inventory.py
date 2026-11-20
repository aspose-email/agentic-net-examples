"""Check versioned publication counts, hashes, links and discovery metadata."""
from pathlib import Path
import hashlib
import json
import subprocess

ROOT = Path(__file__).resolve().parents[2]
manifest = json.loads((ROOT / "v4/manifest.json").read_text())
rows = manifest["examples"]
assert manifest["publication_status"] == "partial" and manifest["catalog_task_total"] == 3308
assert manifest["published_examples"] == len(rows) == 15
paths = {row["path"] for row in rows}
assert len(paths) == len({row["task_id"] for row in rows}) == 15
for row in rows:
    assert (ROOT / row["path"]).is_file()
    assert hashlib.sha256((ROOT / row["path"]).read_bytes()).hexdigest() == row["sha256"]
    assert row["package_version"] == "26.10.0" and row["framework"] == "net8.0"
    for fixture in row["fixtures"]:
        assert hashlib.sha256((ROOT / fixture["path"]).read_bytes()).hexdigest() == fixture["sha256"]
indices = json.loads((ROOT / "index.json").read_text())
search = json.loads((ROOT / "search-index.json").read_text())
readiness = json.loads((ROOT / "readiness.json").read_text())
assert indices["total_examples"] == search["example_count"] == readiness["total_examples"] == 3098
assert indices["total_categories"] == readiness["total_categories"] == len(indices["categories"]) == 20
assert len(search["examples"]) == 3098 and len(search["documents"]) == search["document_count"]
assert paths == {r["path"] for r in search["examples"] if r["path"].startswith("v4/")}
assert paths == {r["path"] for c in indices["categories"] if c["name"].startswith("v4/") for r in c["examples"]}
for item in search["examples"] + search["documents"]:
    assert (ROOT / item["path"]).is_file(), item["path"]
assert len(list((ROOT / "v4").rglob("*.cs"))) == 15
print("Inventory passed: 3083 preserved legacy + 15 V4 sources = 3098; 20 leaf category groups.")
