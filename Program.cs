using Rinha.Internal.AstJson;
using Rinha.Syntax;

var jsonParser = new Parser();
var jsonEmmiter = new Emmiter();

var src = @"
{
  ""name"": ""ata.rinha"",
  ""expression"": {
    ""kind"": ""Binary"",
    ""lhs"": {
      ""kind"": ""Int"",
      ""value"": 1,
      ""location"": {
        ""start"": 0,
        ""end"": 1,
        ""filename"": ""ata.rinha""
      }
    },
    ""op"": ""Add"",
    ""rhs"": {
      ""kind"": ""Int"",
      ""value"": 2,
      ""location"": {
        ""start"": 2,
        ""end"": 3,
        ""filename"": ""ata.rinha""
      }
    },
    ""location"": {
      ""start"": 0,
      ""end"": 3,
      ""filename"": ""ata.rinha""
    }
  },
  ""location"": {
    ""start"": 0,
    ""end"": 1,
    ""filename"": ""ata.rinha""
  }
}
";

var parsed = jsonParser.Parse(src);
var emmited = jsonEmmiter.Emmit(parsed!);
Console.WriteLine(emmited);
