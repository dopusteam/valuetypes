# valuetypes
Contains some frequently used structs, does not presented in System namespace.
1. Date - value type for handle date without time part. Based on DateTime type and provides timeless interface.


## Why not DateTime?
1. If you only need date, do not care about time zones, just work with date
2. If you only need date, do not care about hours, minutes and seconds, just work with date


## Examples
```csharp
var firstDate = new Date(2001, 1, 1);
var secondDate = new Date(2001, 1, 2);

Console.WriteLine(firstDate.ToString()); // "2001-01-01"
Console.WriteLine(secondDate.ToString()); // "2001-01-02"

Console.WriteLine(firstDate == secondDate); // false

var newFirstDate = firstDate.AddDays(1);

Console.WriteLine(newFirstDate.ToString()); // "2001-01-02"

Console.WriteLine(firstDate == secondDate); // false
Console.WriteLine(newFirstDate == secondDate); // true
```
