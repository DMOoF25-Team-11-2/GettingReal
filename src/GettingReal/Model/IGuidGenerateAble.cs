namespace GettingReal.Model;

/// <summary>
/// Interface for classes that can generate a GUID.
/// </summary>
/// <remarks>
/// This interface is used to ensure that classes that implement it can generate a GUID.
/// </remarks>
public interface IGuidGenerateAble
{
    bool DoesGuidExist(Guid guid);
}
