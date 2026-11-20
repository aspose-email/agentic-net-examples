"""Reproduce compile, runtime and native checks for every published V4 source."""
from pathlib import Path
import argparse
import hashlib
import json
import os
import shutil
import subprocess
import sys
import tempfile
from check import ROOT, check


def run(work_root, selected=None):
    manifest = json.loads((ROOT / "manifest.json").read_text())
    rows = []
    env = os.environ.copy()
    env.setdefault("DOTNET_NOLOGO", "1")
    env.setdefault("DOTNET_CLI_TELEMETRY_OPTOUT", "1")
    for entry in manifest["examples"]:
        task = entry["task_id"]
        if selected and task not in selected:
            continue
        directory = work_root / task
        directory.mkdir(parents=True, exist_ok=False)
        source = ROOT.parent / entry["path"]
        assert hashlib.sha256(source.read_bytes()).hexdigest() == entry["sha256"]
        shutil.copyfile(source, directory / "Program.cs")
        for fixture in entry["fixtures"]:
            rel = Path(fixture["runtime_path"])
            assert not rel.is_absolute() and ".." not in rel.parts
            original = ROOT.parent / fixture["path"]
            assert hashlib.sha256(original.read_bytes()).hexdigest() == fixture["sha256"]
            target = directory / rel
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copyfile(original, target)
        (directory / "example.csproj").write_text('''<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework><ImplicitUsings>enable</ImplicitUsings></PropertyGroup><ItemGroup><PackageReference Include="Aspose.Email" Version="26.10.0" /></ItemGroup></Project>''')
        row = {"task_id": task, "source_sha256": entry["sha256"], "framework": "net8.0", "package_version": "26.10.0", "compile_success": False, "runtime_success": False, "native_operation_verified": False}
        try:
            task_env = env.copy()
            task_env.update(entry.get("runtime_environment", {}))
            commands = [("restore", ["dotnet", "restore", "--verbosity", "quiet"]), ("build", ["dotnet", "build", "-c", "Release", "--no-restore", "--verbosity", "quiet"]), ("runtime", ["dotnet", "run", "-c", "Release", "--no-build"])]
            for name, command in commands:
                result = subprocess.run(command, cwd=directory, env=task_env, capture_output=True, text=True, timeout=180 if name != "runtime" else 60)
                (directory / (name + ".log")).write_text(result.stdout + result.stderr)
                if result.returncode:
                    raise RuntimeError(name + " failed: " + (result.stdout + result.stderr)[-3000:])
                if name == "build": row["compile_success"] = True
                elif name == "runtime": row["runtime_success"] = True
            native = check(task, directory)
            row.update(native)
            assert hashlib.sha256((directory / "Program.cs").read_bytes()).hexdigest() == entry["sha256"]
        except Exception as error:
            row["error"] = str(error)
        rows.append(row)
        (work_root / "report.json").write_text(json.dumps({"framework": "net8.0", "package_version": "26.10.0", "catalog_version": "4.0", "selected_source_count": len(rows), "all_passed": all(r["native_operation_verified"] for r in rows), "examples": rows}, indent=2) + "\n")
        print(task, "PASS" if row["native_operation_verified"] else "FAIL", flush=True)
    return rows

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--work-dir", type=Path, help="New output directory outside the repository; keep per-example logs/results")
    parser.add_argument("--task", action="append", help="Optional task ID, repeatable")
    args = parser.parse_args()
    if args.work_dir:
        args.work_dir.mkdir(parents=True, exist_ok=False)
        rows = run(args.work_dir.resolve(), args.task)
    else:
        with tempfile.TemporaryDirectory(prefix="email-v4-validation-") as temp:
            rows = run(Path(temp), args.task)
            print(json.dumps({"examples": rows}, indent=2))
    sys.exit(0 if rows and all(r["native_operation_verified"] for r in rows) else 1)
