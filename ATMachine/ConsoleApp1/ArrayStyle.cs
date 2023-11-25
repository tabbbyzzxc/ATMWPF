using System.Text;

namespace ConsoleApp1
{
    public delegate void XXX(string tttttt);

    class ATM
    {
        public void AddMoney(int money, Action<string> del)
        {
            // bal += money;
            del($"Money added {money}");
        }
    }


    public class ArrayStyle
    {
        public string Decorate(int[] array, Func<int, bool> del)
        {
            var sb = new StringBuilder("[ ");
            for (int i = 0; i < array.Length; i++)
            {
                if (del(array[i]))
                {
                    sb.Append($"({array[i]}), ");
                }
                else
                {
                    sb.Append($"{array[i]}, ");
                }
            }

            sb.Append(']');
            return sb.ToString();
        }
    }
}