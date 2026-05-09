namespace BGInfoPlus.Modules
{
    public class Enums
    {
        public enum StatusSeverityType
        {
            Information = 0,
            Warning = 1,
            Error = 2,
            Debug = 3,
            Fatal = 4
        }
        public enum ElementTypes //For use for the ElementRenderer to determine which type of element to render
        {
            Text,
            Image,
            Box
        }
    }
}
