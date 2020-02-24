# ElevatorPitchGenerator

The project purpose is to generate text like elevator pitch or few pharagraph stories with randomly filled templates. 
The templates can be changed however you want, until the _Init and _EntryPoint JSON-s can be found in the input folder.

Tags and usage:
- [option1|option2|...] - randomly chooses one of the options separated with the '|' character.
- @template tag@ - replaces the tag with one of the options of the given tag.
- $var:value$ - replaces the string with the evaluated string, from the same $var:...$ tag always be evaluated to the same value.
- {var:value} - almost the same, but replaces the string with an empty string at the end, use this for initialization.
