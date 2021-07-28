using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorApp.Model
{
    public interface ICalculation
    {
        double Calculate(double op1, double op2);
    }

    public class AddCalculation : ICalculation
    {
        public double Calculate(double op1, double op2)
        {
            return op1 + op2;
        }
    }
    public class SubCalculation : ICalculation
    {
        public double Calculate(double op1, double op2)
        {
            return op1 - op2;
        }
    }
    public class DivideCalculation : ICalculation
    {
        public double Calculate(double op1, double op2)
        {
            return op1 / op2;
        }
    }
    public class MultiplyCalculation : ICalculation
    {
        public double Calculate(double op1, double op2)
        {
            return op1 * op2;
        }
    }

    public class CalculatorHistoryModel
    {
        public Guid id { get; set; }
        public double op1 { get; set; }
        public double op2 { get; set; }
        public string opp { get; set; }
        public string result { get; set; }
    }

    public static class CalHistory{
        private static List<CalculatorHistoryModel> calculationHistory = new List<CalculatorHistoryModel>();
        private static Dictionary<String, ICalculation> calculationDic = new Dictionary<String, ICalculation>();
       
        public static List<CalculatorHistoryModel> GetCalculationHistory() {
            return calculationHistory;
        }
        public static void AddCalculationHistory(CalculatorHistoryModel obj)
        {
            calculationHistory.Add(obj);
        }
        public static void UpdateCalculationHistory(CalculatorHistoryModel obj)
        {
            int index = calculationHistory.FindIndex(m => m.id == obj.id);
            if (index >= 0)
                calculationHistory[index] = obj;
        }

        public static CalculatorHistoryModel GetCalculationHistoryById(Guid Id)
        {
            return calculationHistory.SingleOrDefault(x => x.id == Id);
        }
        public static void RemoveCalculationHistory(Guid Id)
        {
            var item = calculationHistory.SingleOrDefault(x => x.id == Id);
            if (item != null)
                calculationHistory.Remove(item);
        }
        public static double PerformCalculation(CalculatorModel model)
        {
            return calculationDic[model.opp].Calculate((double)model.op1, (double)model.op2);
        }

        public static void PopulateDic()
        {
            if (!calculationDic.ContainsKey("+"))
                calculationDic.Add("+", new AddCalculation());

            if (!calculationDic.ContainsKey("-"))
                calculationDic.Add("-", new SubCalculation());

            if (!calculationDic.ContainsKey("*"))
                calculationDic.Add("*", new MultiplyCalculation());

            if (!calculationDic.ContainsKey("/"))
                calculationDic.Add("/", new DivideCalculation());
        }
    }

    public class CalculatorModel
    {
        public double? op1 { get; set; }
        public double? op2 { get; set; }
        public string opp { get; set; }
    }
}