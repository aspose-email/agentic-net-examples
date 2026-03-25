# Category Agent Notes

- Folder: programming-email-verification
- Filenames must be meaningful and under 250 characters including `.cs`.
- Use verified Aspose.Email APIs; wrap network/auth/file operations in try/catch and validate inputs.
- Avoid live calls with placeholder credentials/hosts; fail gracefully if required settings are missing.
- Dispose clients/messages/streams with `using`.
