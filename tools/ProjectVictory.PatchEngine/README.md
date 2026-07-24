# Project Victory Patch Engine

## Build

```bash
dotnet build
```

## Preview

```bash
pvpatch preview PV-001.patch
```

## Apply

```bash
pvpatch apply PV-001.patch
```

## Generate

```bash
pvpatch generate original.cs modified.cs PV-001.patch
```

Fluxo oficial do Project-Victory:

Implementation

↓

UnifiedDiffGenerator

↓

Unified Diff (.patch)

↓

Patch Engine

↓

Preview

↓

Backup

↓

Apply