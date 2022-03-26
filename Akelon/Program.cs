using System;
using System.Text;

namespace Akelon
{
    public class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            var r = p.SumDistribute(2000, "300;200;1000;300;700;500", "ПРОП");
            Console.WriteLine(r);
        }
        /// <summary>
        /// Функция распределение указанной суммы с учетом типа распределения
        /// </summary>
        /// <param name="value">Сумма к распределению</param>
        /// <param name="sums">Список сумм через «;» в соответствии с которыми распределяется сумма</param>
        /// <param name="type">Тип распределения</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public string SumDistribute(double value, string sums, string type)
        {
            if (sums == null) throw new ArgumentNullException(nameof(sums));
            var s_d = sums.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            double cur_sum = 0;
            switch (type)
            {
                case "ПРОП":
                    double total_sum = 0;
                    for (int i = 0; i < s_d.Length; i++)
                    {
                        total_sum += double.Parse(s_d[i]);
                    }
                    double factor = value / total_sum;
                    double k = 0;
                    for (int i = 0; i < s_d.Length - 1; i++)
                    {
                        k = Math.Round(double.Parse(s_d[i]) * factor, 2);
                        cur_sum += k;
                        sb.Append(k);
                        sb.Append(';');
                    }
                    sb.Append(Math.Round(value - cur_sum, 2));
                    break;
                case "ПЕРВ":
                    int j = 0;
                    for (; j < s_d.Length; j++)
                    {
                        cur_sum += double.Parse(s_d[j]);
                        if (cur_sum >= value)
                        {
                            sb.Append(Math.Round(double.Parse(s_d[j]) - (cur_sum - value), 2));
                            sb.Append(';');
                            break;
                        }
                        else
                        {
                            sb.Append(double.Parse(s_d[j]));
                            sb.Append(';');
                        }
                    }
                    j++;
                    for (; j < s_d.Length; j++)
                    {
                        sb.Append('0');
                        sb.Append(';');
                    }
                    if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
                    break;
                case "ПОСЛ":
                    int z = s_d.Length - 1;
                    for (; z >= 0; z--)
                    {
                        cur_sum += double.Parse(s_d[z]);
                        if (cur_sum >= value)
                        {
                            sb.Insert(0, ';');
                            sb.Insert(0, Math.Round(double.Parse(s_d[z]) - (cur_sum - value), 2));
                            break;
                        }
                        else
                        {
                            sb.Insert(0, ';');
                            sb.Insert(0, double.Parse(s_d[z]));
                        }
                    }
                    z--;
                    for (; z >= 0; z--)
                    {
                        sb.Insert(0,';');
                        sb.Insert(0,'0');
                    }
                    if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
                    break;
                default:
                    throw new ArgumentException("Type of distribution is not valid");
            }
            return sb.ToString();
        }

        

        private void CorrectLast()
        {
            throw new NotImplementedException();
        }
    }
}
