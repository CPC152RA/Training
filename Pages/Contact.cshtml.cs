using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Smartsheet.Api;
using Smartsheet.Api.Models;

namespace MyRazorForm.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty, Required]
        public string Name { get; set; }

        [BindProperty, Required, EmailAddress]
        public string Email { get; set; }

        public string Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please fill in all required fields.";
                return Page();
            }

            // Replace with your actual Smartsheet API token and IDs
            var smartsheetToken = "8HgWFROTxTavHVa25qqcJzzjzkkJjYIFHQVpU";
            long sheetId = 3801466310446980;
            long nameColumnId =  5948882724540292;
            long emailColumnId = 3697082910855044;

            var smartsheet = new SmartsheetBuilder().SetAccessToken(smartsheetToken).Build();

            var row = new Row
            {
                Cells = new List<Cell>
                {
                    new Cell { ColumnId = nameColumnId, Value = Name },
                    new Cell { ColumnId = emailColumnId, Value = Email }
                }
            };

            try
            {
                var result = await Task.Run(() =>
                    smartsheet.SheetResources.RowResources.AddRows(sheetId, new Row[] { row })
                );
                Message = $"Thank you, {Name}! We will contact you at {Email}.";
            }
            catch (Exception ex)
            {
                Message = $"Failed to submit. {ex.Message}";
            }

            return Page();
        }
    }
}
