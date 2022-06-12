# ZeroCodingTask
ZeroCodingTask for custom configuration.


Coding Assignment

Implement Microsoft.Extensions.Configuration.IConfigurationSource for custom storage format in C#.

Custom storage format specification:
The configuration is defined by a set of text files which represent a node in a configuration hierarchy. Each node file may have child nodes definitions, which defined by file system path to another file, and key/value definitions.

- Each line in file defines either node link to another file, or a key/value pair.
- If line starts with 'FILE:' it defines a node link in a format '{nodeName}:"{file_path}"'.
- Otherwise, line is considered a key/value definition in a format '{key}:"{value}"'

Example structure:

    nodeFoo
    ├─ key123=321
    nodeBar
    ├─ innernode1
    │ ├─ key777=777
    ├─ innernode2
    │ ├─ key888=true
    keyFoo="keyFoo value"
    keyBar="keyBar value"

    root.txt
    ```
    FILE:nodeFoo:"c:\temp\txt3.txt"
    FILE:nodeBar:"c:\users\user\documents\txt5.txt"
    keyFoo:"keyFoo value"
    keyBar:"keyBar value"
    ```

    txt3.txt
    ```
    key123:"321"
    ```

    txt5.txt
    ```
    FILE:innernode1:"c:\temp\txt777.txt"
    FILE:innernode2:"c:\temp\txt888.txt"
    ```

    txt777.txt
    ```
    key777:"777"
    ```

    txt888.txt
    ```
    key888:"true"
    ```

Expected configuration read result for such structure:
    ```
    IConfigurationRoot configurationRoot;
    configurationRoot["keyBar"] == "keyBar value"
    configurationRoot.GetSection("nodeFoo")["key123"] == "321"
    configurationRoot.GetSection("nodeBar").GetSection("innernode2")["key888"] == "true"
    ```

Requirements:
- Support reading configuration key values.
- IConfigurationBuilder extension method for convenient adding implemented configuration source to IConfigurationRoot.

Optional requirements:
- Support writing configuration values.
- Support configuration reload when underlying storage structure or content changes (using IChangeToken).
- Unit tests (not 100% covergage, just few tests for main scenarios would be enough).

Links:
https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfigurationsource
