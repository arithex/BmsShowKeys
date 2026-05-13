# BmsShowKeys
Simple console app to show unassigned keys in a Falcon BMS key file

### Usage:
- run from the Command Prompt .. specify the path to the key file to scan

### Example:

```
> ShowKeys.exe "C:\Falcon BMS 4.38\User\Config\BMS - Auto.key"
```

The output report contains 8 groups, by modifier combos (unshifted, shift, ctrl, shift+ctrl, alt .. etc)

### Limitations:

- the key names are currently hardcoded to standard US keyboard layout
- it doesn't give any special-case consideration for the QWERTY radio commands, or reserved Windows keys like [ctrl+alt+delete] or [alt+tab]
- it currently does not look at keycombo-sequences at all (eg. [alt+C,P] to toggle the pilot on/off)
