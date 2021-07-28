using CalculatorApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorApp.Controllers
{
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("api/Calculator/Calculate")]
        public IActionResult Calculate(CalculatorModel model)
        {
            try
            {
                //Check Validation
                if (model.op1 == null || model.op2 == null || string.IsNullOrEmpty(model.opp) | string.IsNullOrWhiteSpace(model.opp))
                {
                    return BadRequest("Invalid Operation");
                }
                if (model.opp.Trim() == "+" || model.opp.Trim() == "-" || model.opp.Trim() == "*" || model.opp.Trim() == "/")
                {
                    //Perform Calculation
                    var result = CalHistory.PerformCalculation(model);

                    //Maintain History
                    CalculatorHistoryModel calculatorhistoryModel = new CalculatorHistoryModel();
                    calculatorhistoryModel.id = Guid.NewGuid();
                    calculatorhistoryModel.op1 = (double)model.op1;
                    calculatorhistoryModel.op2 = (double)model.op2;
                    calculatorhistoryModel.opp = model.opp;
                    calculatorhistoryModel.result = result.ToString();
                    CalHistory.AddCalculationHistory(calculatorhistoryModel);

                    return Ok(result);
                }
                else
                {
                    return BadRequest("Invalid Operation");
                }
            }
            catch(Exception ex)
            {
                return BadRequest("Invalid Operation");
            }
        }

        [HttpGet]
        [Route("api/Calculator/GetCalculationHistory")]
        public IActionResult GetCalculationHistory()
        {
            var calHis = CalHistory.GetCalculationHistory();
            return Ok(calHis);
        }

        [HttpPut]
        [Route("api/Calculator/UpdateHistory")]
        public IActionResult UpdateHistory(CalculatorHistoryModel model)
        {
            try
            {
                //Check Validation
                if ((model.op1 == 0 && model.op2 == 0) || string.IsNullOrEmpty(model.opp) | string.IsNullOrWhiteSpace(model.opp))
                {
                    return BadRequest("Invalid Operation");
                }
                if (model.opp.Trim() == "+" || model.opp.Trim() == "-" || model.opp.Trim() == "*" || model.opp.Trim() == "/")
                {
                    CalculatorModel m = new CalculatorModel();
                    m.op1 = model.op1;
                    m.op2 = model.op2;
                    m.opp = model.opp;

                    //Perform Calculation
                    var result = CalHistory.PerformCalculation(m);
                    model.result = result.ToString();

                    CalHistory.UpdateCalculationHistory(model);
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Invalid Operation");
                }
            }
            catch
            {
                return BadRequest("Invalid Operation"); ;
            }
        }

        [HttpDelete]
        [Route("api/Calculator/DeleteHistoryById/{id}")]
        public IActionResult DeleteHistoryById(string id)
        {
            CalHistory.RemoveCalculationHistory(new Guid(id));
            return Ok();
        }

        [HttpGet]
        [Route("api/Calculator/GetHistoryById/{id}")]
        public IActionResult GetHistoryById(string id)
        {
            var result = CalHistory.GetCalculationHistoryById(new Guid(id));
            return Ok(result);
        }
    }
}