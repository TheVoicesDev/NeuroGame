using System.Linq;

namespace Fantome.Utilities;

public static class GeneralUtility
{
    public static T GetChildOfType<T>(this Node root, bool recursive = false) where T : Node
    {
        Node[] children = root.GetChildren().ToArray();
        for (int i = 0; i < children.Length; i++)
        {
            Node child = children[i];
            if (child is T tChild)
                return tChild;

            if (!recursive)
                continue;
            
            T result = GetChildOfType<T>(child, recursive:true);
            if (result != null)
                return result;
        }

        return null;
    }
}