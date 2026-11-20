# Aspose.Email V4 examples — partial verified delivery

This slice publishes **15 standalone C# examples** from the **3,308-task V4 catalog**, using **Aspose.Email 26.10.0** and **.NET 8**. It is a partial delivery across three categories. The repository also retains all **3,083 legacy sources** in their original 17 categories; this delivery does not recertify that historical corpus.

Each V4 source is tied to a SHA-256 digest, real input fixtures when required, compile/runtime evidence and native output assertions in [manifest.json](manifest.json). These are repaired task-specific sources, not a claim that every stopped-run failure or the entire V4 catalog is fixed. Missing-input guard exits do not qualify as native operation success.

## Run and verify

Install Python 3 and the .NET 8 SDK, then run from the repository root:

```bash
python3 v4/validation/run.py
```

This uses the official `Aspose.Email` NuGet package pinned to `26.10.0`, stages the committed synthetic fixtures into isolated temporary projects, builds/runs each unchanged source and checks the requested saved output. No mail server, credentials, customer data or new package beyond Aspose.Email is needed. To retain logs, choose a new directory outside the repository:

```bash
python3 v4/validation/run.py --work-dir /tmp/aspose-email-v4-verification
```

Use `--task 95683` to select one task. Each source is independently runnable: create a .NET 8 console project, add `Aspose.Email --version 26.10.0`, copy the selected source as `Program.cs`, and copy that task's `v4/fixtures/<task-id>/` contents into the project's working directory before running. For task 95686, the validator supplies the explicit synthetic `AMP_CSRF_TOKEN=local-fixture-token-95686` environment fixture; manual runs must supply an appropriate token. The source itself does not hardcode a token. Fixtures are small synthetic SDK/test inputs with reserved example.com identities. They are tests, not production input data.

## Verified scope and limits

The checks inspect saved AMP MIME/components and CID image bytes, calendar MSG fields and recipients, ICS events/reminders, PST imports and existing unrelated messages, and MBOX recipients/date/non-draft append behavior. Evaluation builds can add the SDK's evaluation marker. The examples do not certify client rendering, email delivery, remote image availability, live form endpoints or production CSRF verification. Task 95686 uses an explicitly synthetic local CSRF fixture; a production application must supply and validate a server-issued token.

The existing GitHub workflow keeps the legacy 26.1 pipeline path unchanged. V4 paths alone use 26.10, receive their real fixtures and run the native checker after the existing compile/runtime gates. See [local validation evidence](validation/local-report.json) and the PR checks for the corresponding Linux run.

## Example inventory

| Task | Category | Requested operation | Source |
|---|---|---|---|
| 95676 | amp-emails | Instantiate AmpMessage, set HtmlBody with style block, add AmpAccordion with three panels, save as amp_styled_accordion.eml. | [95676.cs](amp-emails/95676.cs) |
| 95679 | amp-emails | Instantiate AmpMessage, set HtmlBody with meta viewport tag, add AmpFitText component, save as amp_viewport_fittext.eml. | [95679.cs](amp-emails/95679.cs) |
| 95681 | amp-emails | Create AmpForm with submit button labeled Subscribe, method POST, add to message, save as amp_form_subscribe.eml. | [95681.cs](amp-emails/95681.cs) |
| 95683 | amp-emails | Create AmpMessage, add AmpCarousel with three images, then add AmpForm below carousel, save as amp_carousel_form.eml. | [95683.cs](amp-emails/95683.cs) |
| 95685 | amp-emails | Instantiate AmpMessage, set HtmlBody with header element, add AmpAccordion with two panels, save as amp_header_accordion.eml. | [95685.cs](amp-emails/95685.cs) |
| 95686 | amp-emails | Create AmpForm with CSRF token hidden field, email text field, and submit button, save as amp_form_csrf.eml. | [95686.cs](amp-emails/95686.cs) |
| 95699 | archives | Load messages from "LegalMails" directory using a local GetMessages iterator and add them to "Legal" folder. | [95699.cs](archives/95699.cs) |
| 95799 | archives | Retrieve only subject and date fields using EnumerateMessageInfo and write them to a summary text file. | [95799.cs](archives/95799.cs) |
| 95805 | archives | Create a MailMessage with To, Cc, and Bcc fields and store it in Messages.mbox. | [95805.cs](archives/95805.cs) |
| 95837 | archives | Append a MailMessage with IsDraft set to false to an existing MBOX file to ensure draft restriction compliance. | [95837.cs](archives/95837.cs) |
| 95552 | calendar | Create a new appointment with HTML description and save it as a .msg file using AppointmentMsgSaveOptions. | [95552.cs](calendar/95552.cs) |
| 95562 | calendar | Collect a set of appointment objects and write them all to one .ics file using CalendarWriter. | [95562.cs](calendar/95562.cs) |
| 95584 | calendar | Use CalendarWriter to write a collection of appointments that include display and audio reminders to a .ics file. | [95584.cs](calendar/95584.cs) |
| 95608 | calendar | Load a local MSG meeting as MapiCalendar, add attendees with MapiRecipientCollection and MapiRecipientType.MAPI_TO, and save the updated MSG. | [95608.cs](calendar/95608.cs) |
| 95610 | calendar | Load a local MSG meeting as MapiCalendar and save it as ICS with default options. | [95610.cs](calendar/95610.cs) |
