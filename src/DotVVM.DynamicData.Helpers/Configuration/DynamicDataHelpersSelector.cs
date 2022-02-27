namespace DotVVM.DynamicData.Helpers.Configuration;

public class DynamicDataHelpersSelector
{
    public Type SelectorItemType { get; }

    public Type SelectorItemServiceType { get; }

    public DynamicDataHelpersSelector(Type selectorItemType, Type selectorItemServiceType)
    {
        SelectorItemType = selectorItemType;
        SelectorItemServiceType = selectorItemServiceType;
    }
}