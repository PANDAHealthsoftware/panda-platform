namespace PANDAView.Models;

public class EnumDropdownItem<T>
{
    public T Value { get; set; } = default!;
    public string Text { get; set; } = string.Empty;
}