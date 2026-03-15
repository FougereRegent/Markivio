using System.Text.RegularExpressions;
using Markivio.Domain.Exceptions;

namespace Markivio.Domain.ValueObject;


public sealed class TagValueObject : BaseValueObject {

	public string Name {get;set;}
	public string Color {get;set;}

	private TagValueObject() {}

	public TagValueObject(string name, string color) {
		if(string.IsNullOrEmpty(name))
			throw new EmptyException("tag name cannot be empty", "EMPTY_TAGNAME");
		if(string.IsNullOrEmpty(color))
			throw new EmptyException("tag color cannot be empty", "EMPTY_COLORTAG");

		if(!Regex.IsMatch(name, @"^[A-Za-zÀ-ÿà-ÿ\-\'’ 0-9 &#`\-_]{1,25}$"))
			throw new PatternException($"{name} didn't respect the format", "FORMAT_TAGNAME");

		if(!Regex.IsMatch(color, @"^#[A-Fa-f0-9]{6}$"))
			throw new PatternException("tag should be format to #FFFFFF", "FORMAT_COLOTTAG");

		Name = name;
		Color = color;
	}

    protected override IEnumerable<object> GetAtomicValues()
    {
		yield return Name;
		yield return Color;
    }
}
