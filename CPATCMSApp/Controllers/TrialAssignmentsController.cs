using System;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using CPATCMSApp.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CPATCMSApp.Models.TrialAssignments;

namespace CPATCMSApp.Controllers
{
    public class TrialAssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrialAssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(DateTime? assignmentDate)
        {
            ViewData["AssignmentDate"] = assignmentDate;
            return View();
        }

        [HttpGet]
        public IActionResult FetchFromCTMS()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FetchFromCTMS(DateTime assignmentDate)
        {
            var url = "http://122.152.54.179/dataInfo/today_assign.php?dt=" + assignmentDate.ToString("yyyy-MM-dd");
            var jsonDataFromCTMSLink = new WebClient().DownloadString(url);
            dynamic deserializedObjects = JsonConvert.DeserializeObject(jsonDataFromCTMSLink);

            foreach (var singleObject in deserializedObjects)
            {
                string containerNo = singleObject.Container_No.ToString().Trim();
                //var bay = singleObject.Bay.ToString().Trim(); //unused
                //var yardNo = singleObject.Yard_No.ToString().Trim(); //unused
                //var assignmentDate = singleObject.Assignment_Dt.ToString().Trim(); //unused
                if (singleObject.Block_No.ToString().Trim().ToUpper() == "NCY")
                {
                    var trialAssignmentItem = await _context.TrialAssignmentItems.Where(m => m.ContainerNumber == containerNo && m.CreationTime == assignmentDate).FirstOrDefaultAsync();

                    if (trialAssignmentItem == null)
                    {
                        var newTrialAssignmentItem = new TrialAssignmentItem
                        {
                            YardId = 2,
                            BlockName = singleObject.Block_No.ToString().Trim(),
                            CnFCode = singleObject.CNF_Code.ToString().Trim(),
                            CnFName = singleObject.CNF_Agent.ToString().Trim(),
                            Vessel = singleObject.Vessel_Name.ToString().Trim(),
                            ImpReg = singleObject.Import_Rotation.ToString().Trim(),
                            MLO = singleObject.MLO.ToString().Trim(),
                            ContainerNumber = containerNo,
                            AssignmentType = singleObject.Assignment_Type.ToString().Trim(),
                            Size = Convert.ToDouble(singleObject.Cont_Size.ToString().Trim()),
                            Height = Convert.ToDouble(singleObject.Cont_Height.ToString().Trim()),
                            LineNumber = singleObject.line_no.ToString().Trim(),
                            Dst = singleObject.Delivery_st.ToString().Trim(),
                            Remarks = singleObject.remarks.ToString().Trim(),
                            VerifyNumber = singleObject.Verify_No.ToString().Trim(),
                            ExitNumber = singleObject.Exit_No.ToString().Trim(),
                            CreationTime = assignmentDate
                        };

                        _context.Add(newTrialAssignmentItem);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}