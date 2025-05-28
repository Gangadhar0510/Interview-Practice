
function exportTableToExcel() {
    var fromDate = $('#logFromDate').val();
    var toDate = $('#logToDate').val();
    var jobName = $('#ParentJobName').val() || "";
    var errorType = $('#errorType').val();

    $.ajax({
        url: '/OperationsManager/ExportToExcel',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            fromDate: fromDate,
            toDate: toDate,
            jobName: jobName,
            errorType: errorType
        }),
        success: function (response) {
            if (response.success && response.fileUrl) {
                // Trigger download
                window.location.href = response.fileUrl;
            } else {
                alert(response.message || "Export failed.");
            }
        },
        error: function (xhr) {
            alert("An error occurred during export.");
        }
    });
}
function exportToExcelWithColors() {
  var table = $('#example').DataTable();

  // Create a new hidden table to export
  var exportTable = $('<table>');

  // Copy the header
  var thead = $('<thead>').append($('#example thead').html());
  exportTable.append(thead);

  // Copy rows with inline background color
  $('#example tbody tr').each(function () {
    var $row = $(this).clone();
    var bgColor = $(this).css('background-color');

    $row.find('td').css('background-color', bgColor);
    exportTable.append($row);
  });

  // Use SheetJS (XLSX) to export
  var wb = XLSX.utils.table_to_book(exportTable[0], {sheet: "Sheet1"});
  XLSX.writeFile(wb, "ColoredExport.xlsx");
}


const fromDateInput = document.getElementById('FromDate');
const toDateInput = document.getElementById('ToDate');
const today = new Date();
const todayStr = today.toISOString().slice(0, 10);

function formatDateToStr(date) {
  return date.toISOString().slice(0, 10);
}

function isValidDateInRange(dateStr, minStr, maxStr) {
  const date = new Date(dateStr);
  const min = new Date(minStr);
  const max = new Date(maxStr);
  return date >= min && date <= max;
}

function setToDateLimits(fromDateStr) {
  if (!fromDateStr) return;

  const fromDate = new Date(fromDateStr);
  let maxToDate = new Date(fromDate);
  maxToDate.setFullYear(maxToDate.getFullYear() + 2);
  if (maxToDate > today) maxToDate = today;

  toDateInput.min = fromDateStr;
  toDateInput.max = formatDateToStr(maxToDate);

  // Reset if existing value is invalid
  if (toDateInput.value && !isValidDateInRange(toDateInput.value, toDateInput.min, toDateInput.max)) {
    toDateInput.value = '';
    toDateInput.classList.remove('is-valid');
    toDateInput.classList.add('is-invalid');
  }
}

function setFromDateLimits(toDateStr) {
  if (!toDateStr) return;

  const toDate = new Date(toDateStr);
  let minFromDate = new Date(toDate);
  minFromDate.setFullYear(minFromDate.getFullYear() - 2);

  fromDateInput.min = formatDateToStr(minFromDate);
  fromDateInput.max = toDateStr;

  if (fromDateInput.value && !isValidDateInRange(fromDateInput.value, fromDateInput.min, fromDateInput.max)) {
    fromDateInput.value = '';
    fromDateInput.classList.remove('is-valid');
    fromDateInput.classList.add('is-invalid');
  }
}

function validateInput(inputElement) {
  const val = inputElement.value;
  if (!val || !inputElement.min || !inputElement.max) return;

  if (isValidDateInRange(val, inputElement.min, inputElement.max)) {
    inputElement.classList.remove('is-invalid');
    inputElement.classList.add('is-valid');
  } else {
    inputElement.value = '';
    inputElement.classList.remove('is-valid');
    inputElement.classList.add('is-invalid');
  }
}

// Combined listeners for change + input on FromDate
fromDateInput.addEventListener('change', () => {
  setToDateLimits(fromDateInput.value);
  validateInput(fromDateInput);
});

fromDateInput.addEventListener('input', () => {
  setToDateLimits(fromDateInput.value);
  validateInput(fromDateInput);
});

// Combined listeners for change + input on ToDate
toDateInput.addEventListener('change', () => {
  setFromDateLimits(toDateInput.value);
  validateInput(toDateInput);
});

toDateInput.addEventListener('input', () => {
  setFromDateLimits(toDateInput.value);
  validateInput(toDateInput);
});




function setFromDateLimits(toDateStr) {
  if (!toDateStr) {
    fromDateInput.min = ''; // or earliest date you allow
    fromDateInput.max = todayStr;
    return;
  }
  const toDate = new Date(toDateStr);
  let minFromDate = new Date(toDate);
  minFromDate.setFullYear(minFromDate.getFullYear() - 2);
  // Optional: set your earliest allowed date if needed

  fromDateInput.min = minFromDate.toISOString().slice(0, 10);
  fromDateInput.max = toDateStr;

  // If current FromDate is out of range, clear it
  if (fromDateInput.value) {
    const fromDate = new Date(fromDateInput.value);
    if (fromDate < minFromDate || fromDate > toDate) {
      fromDateInput.value = '';
    }
  }
}

toDateInput.addEventListener('change', () => {
  setFromDateLimits(toDateInput.value);

  if (toDateInput.value) {
    toDateInput.classList.add('is-valid');
    toDateInput.classList.remove('is-invalid');
  } else {
    toDateInput.classList.remove('is-valid');
  }
});


const fromDateInput = document.getElementById('FromDate');
const toDateInput = document.getElementById('ToDate');
const today = new Date();
const todayStr = today.toISOString().slice(0, 10);

function setToDateLimits(fromDateStr) {
  if (!fromDateStr) {
    toDateInput.min = '';
    toDateInput.max = todayStr;
    return;
  }
  const fromDate = new Date(fromDateStr);
  let maxToDate = new Date(fromDate);
  maxToDate.setFullYear(maxToDate.getFullYear() + 2);
  if (maxToDate > today) maxToDate = today;

  toDateInput.min = fromDateStr;
  toDateInput.max = maxToDate.toISOString().slice(0, 10);

  // If current ToDate is out of range, clear it
  if (toDateInput.value) {
    const toDate = new Date(toDateInput.value);
    if (toDate < fromDate || toDate > maxToDate) {
      toDateInput.value = '';
    }
  }
}

fromDateInput.addEventListener('change', () => {
  setToDateLimits(fromDateInput.value);

  // Optional: update validation styles
  if (fromDateInput.value) {
    fromDateInput.classList.add('is-valid');
    fromDateInput.classList.remove('is-invalid');
  } else {
    fromDateInput.classList.remove('is-valid');
  }
});


$(document).on('click', '#errorCategoryEdit', function () {
    var form = $('#EditErrorCategoryForm');
    var formData = form.serialize();

    $.ajax({
        type: 'POST',
        url: '/Error/Edit',
        data: formData,
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (response.success) {
                alert(response.message);
                $('#AssignErrorMain').modal('hide');
            } else {
                alert("Failed: " + response.message);
            }
        },
        error: function () {
            alert("Something went wrong.");
        }
    });
});

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class RawOpenAITest
{
    public async Task<string> CallOpenAI(string prompt)
    {
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var httpClient = new HttpClient(httpClientHandler);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_api_key");

        var content = new StringContent($@"{{
            ""model"": ""text-davinci-003"",
            ""prompt"": ""{prompt}"",
            ""max_tokens"": 150
        }}", System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("https://api.openai.com/v1/completions", content);
        return await response.Content.ReadAsStringAsync();
    }
}

<div class="col-md-3 d-flex align-items-center fromDate">
    <label asp-for="ExportFromDate" class="form-label me-2 mb-0">From:</label>
    <div class="input-group date" id="datetimepicker1" data-td-target-input="nearest" data-td-target-toggle="nearest">
        <input type="text" asp-for="ExportFromDate" class="form-control form-control-sm datetimepicker-input"
               data-td-target="#datetimepicker1" id="ExportFromDate" />
        <span class="input-group-text" data-td-target="#datetimepicker1" data-td-toggle="datetimepicker">
            <i class="fa fa-calendar"></i>
        </span>
    </div>
</div>

<div class="col-md-3 d-flex align-items-center toDate ms-2">
    <label asp-for="ExportToDate" class="form-label me-2 mb-0">To:</label>
    <div class="input-group date" id="datetimepicker2" data-td-target-input="nearest" data-td-target-toggle="nearest">
        <input type="text" asp-for="ExportToDate" class="form-control form-control-sm datetimepicker-input"
               data-td-target="#datetimepicker2" id="ExportToDate" />
        <span class="input-group-text" data-td-target="#datetimepicker2" data-td-toggle="datetimepicker">
            <i class="fa fa-calendar"></i>
        </span>
    </div>
</div>


<div class="col-md-4 mb-3">
    <label for="ExportFromDate" class="form-label">From DateTime:</label>
    <div class="input-group" id="datetimepicker1" data-td-target-input="nearest" data-td-target-toggle="nearest">
        <input asp-for="ExportFromDate" type="text" class="form-control" data-td-target="#datetimepicker1" />
        <span class="input-group-text" data-td-target="#datetimepicker1" data-td-toggle="datetimepicker">
            <i class="fa fa-calendar"></i>
        </span>
    </div>
</div>

div 
    
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@eonasdan/tempus-dominus@6.7.4/dist/css/tempus-dominus.min.css" />
<script src="https://cdn.jsdelivr.net/npm/@eonasdan/tempus-dominus@6.7.4/dist/js/tempus-dominus.min.js"></script>

<div class="input-group" id="datetimepicker1" data-td-target-input="nearest" data-td-target-toggle="nearest">
    <input id="ExportFromDate" name="ExportFromDate" type="text" class="form-control form-control-sm" data-td-target="#datetimepicker1"/>
    <span class="input-group-text" data-td-target="#datetimepicker1" data-td-toggle="datetimepicker">
        <i class="fa fa-calendar"></i>
    </span>
</div>

<script>
    new tempusDominus.TempusDominus(document.getElementById('datetimepicker1'), {
        display: {
            components: {
                calendar: true,
                date: true,
                month: true,
                year: true,
                decades: true,
                hours: true,
                minutes: true,
                seconds: true
            }
        }
    });
</script>

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>GenAI Dashboard (Simulated)</title>
    <script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@3.1.14/dist/browser/signalr.min.js"></script>
    <script src="~/js/site.js"></script>
</head>
<body>
    <h2>Enter your prompt:</h2>
    <input type="text" id="queryInput" placeholder="Ask anything..." />
    <button onclick="submitQuery()">Submit</button>

    <h3>AI Response:</h3>
    <p id="result">Waiting for response...</p>
</body>
</html>


const connection = new signalR.HubConnectionBuilder()
    .withUrl("/dashboardHub")
    .build();

connection.on("ReceiveMessage", function (message) {
    document.getElementById("result").innerText = message;
});

connection.start().catch(err => console.error(err.toString()));

function submitQuery() {
    const query = document.getElementById("queryInput").value;

    fetch("/Dashboard/ProcessQuery", {
        method: "POST",
        body: JSON.stringify({ userQuery: query }),
        headers: {
            "Content-Type": "application/json"
        }
    });
}



function validateExportDates() {
    const fromInput = document.getElementById('ExportFromDate');
    const toInput = document.getElementById('ExportToDate');

    const fromDate = new Date(fromInput.value);
    const toDate = new Date(toInput.value);

    // Clear previous states
    fromInput.classList.remove('is-valid', 'is-invalid');
    toInput.classList.remove('is-valid', 'is-invalid');

    if (!fromInput.value || !toInput.value || isNaN(fromDate) || isNaN(toDate)) {
        return; // Skip validation if dates are incomplete
    }

    if (fromDate >= toDate) {
        fromInput.classList.add('is-invalid');
        toInput.classList.add('is-invalid');
    } else {
        fromInput.classList.add('is-valid');
        toInput.classList.add('is-valid');
    }
}

// Trigger on input (change, blur, keyup etc.)
document.getElementById('ExportFromDate').addEventListener('change', validateExportDates);
document.getElementById('ExportToDate').addEventListener('change', validateExportDates);




General Questions for All ADOs
Input Format:

How will the list of cases be provided (CSV upload, textbox input, file import)?

What fields are required for each case (e.g., CaseID, AuthorizationID)?

UI Requirements:

Should each functionality be on a separate page/tab, or all together?

Do users need to see a confirmation screen before execution?

Should users see the results (success/failure) for each case?

Permissions/Security:

Who is authorized to perform these actions?

Should actions be logged (audit trail)?

Error Handling:

What should happen if a case is not found?

Should partial success be allowed?

Specific ADO Questions
ADO 9416101: Remove cases from ExportFileStaging table
Is the case identified by CaseID or another field?

Should the case be soft-deleted (e.g., mark as removed) or physically deleted?

Should this action be reversible (e.g., undo option)?

ADO 941616: Remove cases from Export270Header table
Same questions as 9416101 apply here.

Is there a foreign key relationship we need to be aware of before deleting?

Any dependencies or validations before deletion?

ADO 941619: Update tblAuthorization table – set ReexportY and ManualExportY flags
Should we set both flags (ReexportY & ManualExportY) to Y, or only update them if currently N?

Are there any business rules on when these flags can be updated (e.g., case must be in a certain status)?

Should changes be timestamped or logged?

ADO 941622: Add cases to exportblockingQueue table
What fields need to be inserted into this table?

Are we inserting new records or updating existing ones?

Any validations before inserting (e.g., case must not already be in queue)?

Optional Enhancements
Should there be a summary report or export option after processing?

Do we need a history log screen showing past actions per user?

Let me know if you want help drafting the UI mockup or backend logic too — we can go step by step.


    


document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("reconciliationForm");
    const exportSelect = document.getElementById("ExportProcessName");

    form.addEventListener("submit", function (e) {
        // Clear previous invalid state
        exportSelect.classList.remove("is-invalid");

        // Validate
        if (!exportSelect.value || exportSelect.value === "") {
            exportSelect.classList.add("is-invalid");
            e.preventDefault(); // prevent form submission
        }
    });
});

const chartContainer = document.createElement('div');
        chartContainer.className = 'container row';

        chartContainer.innerHTML = `
            <div class="col-md-6">
                <canvas id="data-canvas1"></canvas>
            </div>
            <div class="col-md-6">
                <canvas id="data-canvas2"></canvas>
            </div>
        `;

        document.body.appendChild(chartContainer);


<form asp-controller="Reconciliation" asp-action="Index" method="post">
    <div class="container mt-4">
        <div class="row g-3 align-items-center">

            <div class="col-md-6 d-flex align-items-center">
                <label asp-for="ExportProcessName" class="form-label me-2 mb-0" style="white-space: nowrap;">Export Process:</label>
                <select asp-for="ExportProcessName" class="form-select w-100" name="ExportProcessName">
                    <option value="">Select</option>
                    @foreach (dynamic item in Model.ExportProcesses)
                    {
                        if (item.Name == Model.ExportProcessName)
                        {
                            <option value="@item.Name" title="@item.Name" selected>
                                @item.Name - @item.ExportProcessDescription
                            </option>
                        }
                        else
                        {
                            <option value="@item.Name" title="@item.Name">
                                @item.Name - @item.ExportProcessDescription
                            </option>
                        }
                    }
                </select>
            </div>

            <div class="col-md-3 d-flex align-items-center">
                <label asp-for="ExportFromDate" class="form-label me-2 mb-0">From:</label>
                <input asp-for="ExportFromDate" type="datetime-local" class="form-control" name="ExportFromDate" />
            </div>

            <div class="col-md-3 d-flex align-items-center">
                <label asp-for="ExportToDate" class="form-label me-2 mb-0">To:</label>
                <input asp-for="ExportToDate" type="datetime-local" class="form-control" name="ExportToDate" />
            </div>

            <div class="col-md-12 mt-3">
                <button type="submit" class="btn btn-warning">Show Summary</button>
            </div>

        </div>
    </div>
</form>


function throttle(func, delay) {
    let lastCall = 0;
    return function (...args) {
        const now = new Date().getTime();
        if (now - lastCall >= delay) {
            lastCall = now;
            func(...args);
        }
    };
}

function clearAuthorizationFields() {
    const authId = document.getElementById('authorizationIds');
    const submitButton = document.getElementById('SubmitRecalculateData');

    if (authId) {
        authId.value = "";
        authId.classList.remove('is-valid', 'is-invalid');

        $(".failedResponse").hide();
        $(".successRespone").hide();
        $(".authNotExistRespone").hide();

        authId.disabled = false;
        submitButton.disabled = false;

        authId.removeAttribute("data-original-title");
        console.log("Clear button clicked and fields reset!");
    }
}

// Apply throttling (limit execution to once per 2 seconds)
const throttledClear = throttle(clearAuthorizationFields, 2000);

$("#clearButton").on('click', throttledClear);


<!-- Bootstrap Modal -->
<div class="modal fade" id="duplicateModal" tabindex="-1" aria-labelledby="duplicateModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="duplicateModalLabel">Duplicate Authorization IDs Found</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <p>The following duplicate authorization IDs were found:</p>
        <p id="duplicateList" class="text-danger fw-bold"></p>
        <p>Do you want to proceed with unique values?</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
        <button type="button" class="btn btn-primary" id="confirmProceed">Yes, Proceed</button>
      </div>
    </div>
  </div>
</div>

$(document).ready(function () {
    $("#recalculateForm").submit(function (event) {
        event.preventDefault(); // Prevent default form submission

        var authorizationIds = $("#authorizationIds").val().trim();

        if (!authorizationIds) {
            ToastrResponseAllMessage("Please enter at least one authorization ID.");
            return;
        }

        // Split values by comma and remove empty values
        let authArray = authorizationIds.split(',').map(id => id.trim()).filter(id => id !== "");

        // Find duplicates
        let uniqueAuthSet = new Set();
        let duplicateAuths = [];

        authArray.forEach(id => {
            if (uniqueAuthSet.has(id)) {
                duplicateAuths.push(id);
            } else {
                uniqueAuthSet.add(id);
            }
        });

        // If duplicates found, show Bootstrap modal
        if (duplicateAuths.length > 0) {
            $("#duplicateList").text(duplicateAuths.join(', '));
            $("#duplicateModal").modal("show");
        } else {
            sendAjaxRequest([...uniqueAuthSet].join(',')); // No duplicates, proceed directly
        }
    });

    // Handle "Yes, Proceed" button click in modal
    $("#confirmProceed").click(function () {
        let uniqueAuthArray = [...new Set($("#authorizationIds").val().split(',').map(id => id.trim()).filter(id => id !== ""))];
        
        // Update input field with unique values
        $("#authorizationIds").val(uniqueAuthArray.join(', '));

        // Close the modal
        $("#duplicateModal").modal("hide");

        // Send AJAX request
        sendAjaxRequest(uniqueAuthArray.join(', '));
    });

    function sendAjaxRequest(cleanedAuthorizationIds) {
        $('#spinner').show();

        $.ajax({
            url: "/Actions/RecalculateData",
            type: "POST",
            data: { authorizationIds: cleanedAuthorizationIds }, 
            success: function (response) {
                $('#spinner').hide();
                ToastrResponseAllMessage("Data recalculated successfully.");
            },
            error: function () {
                $('#spinner').hide();
                ToastrResponseAllMessage("An error occurred. Please try again.");
            }
        });
    }
});

$(document).ready(function () {
    // Initialize DataTables for each table
    var tables = {
        successTable: $('#successTable').DataTable(),
        AuthNotExistTable: $('#AuthNotExistTable').DataTable(),
        failedAuthTable: $('#failedAuthTable').DataTable()
    };

    // Copy Button Click Event
    $(document).on('click', '.copy-btn', function () {
        var tableId = $(this).data('table'); // Get table ID
        var table = tables[tableId];

        // Get Authorization ID column (assuming it's the first column index 0)
        var authIds = table.column(0).data().toArray().join('\n');

        // Copy to clipboard
        var tempInput = $('<textarea>').val(authIds).appendTo('body').select();
        document.execCommand('copy');
        tempInput.remove();

        alert('Authorization IDs copied from ' + tableId);
    });

    // Excel Button Click Event
    $(document).on('click', '.download-btn', function () {
        var tableId = $(this).data('table'); // Get table ID
        var table = tables[tableId];

        var wb = XLSX.utils.book_new();
        var ws_data = [];

        // Extract headers
        var headers = [];
        $('#' + tableId + ' thead th').each(function () {
            headers.push($(this).text().trim());
        });
        ws_data.push(headers);

        // Extract data
        table.rows().every(function () {
            ws_data.push(this.data());
        });

        // Create Worksheet
        var ws = XLSX.utils.aoa_to_sheet(ws_data);
        XLSX.utils.book_append_sheet(wb, ws, tableId);

        // Save as Excel File
        XLSX.writeFile(wb, tableId + '_Data.xlsx');
    });
});


<div class="card-header">
    <div class="row">
        <div class="col-md-2">
            <form id="recalculateForm">
                <div>
                    <textarea class="form-control" id="authorizationIds" name="authorizationIds"
                        rows="5" cols="30"
                        placeholder="Enter an authorization ID or list of authorization IDs">
EXXXX-XXX
EXXXX-XXX
                    </textarea>
                </div>

                <br />

                <div>
                    <button id="SubmitRecalculateData" type="submit" class="btn btn-primary btn-flat btn-sm margins">
                        Submit
                    </button>
                    <button type="button" id="clearButton" class="btn btn-secondary btn-flat btn-sm m-2">
                        Clear
                    </button>
                </div>
            </form>
        </div>
    </div>
</div>

<!-- jQuery (required for AJAX) -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<script>
    $(document).ready(function () {
        $("#recalculateForm").submit(function (event) {
            event.preventDefault(); // Prevent default form submission

            var authorizationIds = $("#authorizationIds").val().trim();
            if (!authorizationIds) {
                alert("Please enter at least one authorization ID.");
                return;
            }

            $.ajax({
                url: "/Actions/RecalculateData", // Adjust the URL as needed
                type: "POST",
                data: { authorizationIds: authorizationIds },
                success: function (response) {
                    alert("Recalculation successful!");
                },
                error: function () {
                    alert("Error while recalculating data.");
                }
            });
        });

        $("#clearButton").click(function () {
            $("#authorizationIds").val(""); // Clear textarea
        });
    });
</script>




$("details").on("toggle", function () {
        let timeline = $(this).closest(".timeline");
        let allDetails = timeline.find("details");
        let arrowIcon = timeline.find(".toggle-arrow");

        let areAllOpen = allDetails.length > 0 && allDetails.filter(function () {
            return $(this).prop("open");
        }).length === allDetails.length;

        if (areAllOpen) {
            arrowIcon.removeClass("fa-chevron-right").addClass("fa-chevron-down");
        } else {
            arrowIcon.removeClass("fa-chevron-down").addClass("fa-chevron-right");
        }
    });


$(document).ready(function () {
    $(".toggle-timeline").on("click", function () {
        let timeline = $(this).closest(".timeline"); // Get the clicked timeline
        let eventGroup = timeline.find(".event-group"); // Find events inside this timeline
        let arrowIcon = $(this).find(".toggle-arrow"); // Find arrow icon inside this timeline

        // Toggle visibility of event group
        eventGroup.slideToggle();

        // Toggle arrow direction
        arrowIcon.toggleClass("fa-chevron-right fa-chevron-down");

        // Open/Close all details within this timeline only
        let allDetails = eventGroup.find("details");
        let isAnyOpen = allDetails.filter(function () {
            return $(this).prop("open");
        }).length > 0;

        if (isAnyOpen) {
            allDetails.prop("open", false); // Close all events in this timeline
        } else {
            allDetails.prop("open", true); // Open all events in this timeline
        }
    });
});

$(".toggle-timeline").on("click", function () {
        let eventGroup = $(this).next(".event-group");
        let icon = $(this).find("i");

        eventGroup.slideToggle(); // Show/Hide events

        // Toggle arrow direction
        if (icon.hasClass("fa-chevron-right")) {
            icon.removeClass("fa-chevron-right").addClass("fa-chevron-down"); // Change to ▼
        } else {
            icon.removeClass("fa-chevron-down").addClass("fa-chevron-right"); // Change to ▶
        }
    });

$(".timeline-group[data-date='" + date + "'] .timeline-item").each(function () {
            var eventText = $(this).find("details").text().trim();
            allEvents.push(eventText);
        });



$(document).on("mousedown", ".timeline-item", function (event) {
            if ($(event.target).is(".copy-btn, .download-btn, .time i, .time b")) {
                event.preventDefault(); // Stops <details> expansion
            }
        });


<script>
    $(document).ready(function () {
        // Prevent expanding <details> when clicking Copy or Download
        $(document).on("click", ".copy-btn, .download-btn", function (event) {
            event.stopPropagation();
        });

        // Copy to Clipboard
        $(document).on("click", ".copy-btn", function () {
            let eventData = $(this).closest(".timeline-item").find(".event-data").text().trim();
            navigator.clipboard.writeText(eventData).then(() => {
                alert("Copied to clipboard!");
            }).catch(err => console.error("Copy failed:", err));
        });

        // Download as Word Document
        $(document).on("click", ".download-btn", function () {
            let eventData = $(this).closest(".timeline-item").find(".event-data").text().trim();
            let blob = new Blob(["\ufeff" + eventData], { type: "application/msword" });
            let link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "EventData.doc";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        });
    });
</script>



<script>
    document.addEventListener("click", function (event) {
        // Prevent <details> from expanding when clicking Copy or Download
        if (event.target.closest(".copy-btn, .download-btn")) {
            event.stopPropagation();
        }

        // Copy Event Data
        if (event.target.closest(".copy-btn")) {
            let eventData = event.target.closest(".timeline-item").querySelector(".event-data").innerText;
            navigator.clipboard.writeText(eventData).then(() => {
                alert("Copied to clipboard!");
            }).catch(err => console.error("Copy failed:", err));
        }

        // Download as Word
        if (event.target.closest(".download-btn")) {
            let eventData = event.target.closest(".timeline-item").querySelector(".event-data").innerText;
            let blob = new Blob(["\ufeff" + eventData], { type: "application/msword" });
            let link = document.createElement("a");
            link.href = URL.createObjectURL(blob);
            link.download = "EventData.doc";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    });
</script>


<!-- Timeline Item -->
<div class="timeline-item">
    <span class="time">
        <i class="fas fa-clock text-dark"></i>&nbsp; <b>@busEvent.SentDate.ToShortTimeString()</b>
    </span>

    <details>
        <summary class="card-header">
            <i class="fas fa-bell text-warning"></i> <!-- Notification icon -->
            @busEvent.Name
        </summary>

        <!-- Buttons: Copy & Download -->
        <div class="mt-2 float-right">
            <!-- Copy Button -->
            <button class="btn btn-sm text-primary copy-btn" data-toggle="tooltip" title="Copy">
                <i class="fas fa-copy"></i> Copy
            </button>

            <!-- Download Button -->
            <button class="btn btn-sm text-success download-btn" data-toggle="tooltip" title="Download as Word">
                <i class="fas fa-file-word"></i> Word
            </button>
        </div>

        <!-- Event Data -->
        <div class="card-body p-1" style="max-height:400px; overflow:auto;">
            <pre>@busEvent.EventData</pre>
        </div>
    </details>
</div>

<script>
    document.addEventListener("DOMContentLoaded", function () {
        // Prevent expanding when clicking Copy or Download
        document.querySelectorAll(".copy-btn, .download-btn").forEach(btn => {
            btn.addEventListener("click", function (event) {
                event.stopPropagation(); // Prevent <details> expansion
            });
        });

        // Copy Event Data
        document.querySelectorAll(".copy-btn").forEach(btn => {
            btn.addEventListener("click", function () {
                const eventData = this.closest(".timeline-item").querySelector("pre").innerText;
                navigator.clipboard.writeText(eventData).then(() => {
                    alert("Copied to clipboard!");
                });
            });
        });

        // Download Event Data as Word
        document.querySelectorAll(".download-btn").forEach(btn => {
            btn.addEventListener("click", function () {
                const eventData = this.closest(".timeline-item").querySelector("pre").innerText;
                const blob = new Blob([eventData], { type: "application/msword" });
                const link = document.createElement("a");
                link.href = URL.createObjectURL(blob);
                link.download = "EventData.doc";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            });
        });
    });
</script>


$(document).ready(function () {
    // Copy Individual Event
    $(".copy-btn").on("click", function () {
        var eventData = $(this).closest(".timeline-item").find(".event-content").text().trim();
        var tempTextarea = $("<textarea>").val(eventData).appendTo("body").select();
        document.execCommand("copy");
        tempTextarea.remove();
        alert("Event copied to clipboard!");
    });

    // Copy All Events
    $("#copyAllBtn").on("click", function () {
        var allText = "";
        $(".timeline-item").each(function () {
            var eventName = $(this).find("summary").text().trim();
            var eventData = $(this).find(".event-content").text().trim();
            allText += eventName + "\n" + eventData + "\n\n";
        });

        var tempTextarea = $("<textarea>").val(allText).appendTo("body").select();
        document.execCommand("copy");
        tempTextarea.remove();
        alert("All events copied to clipboard!");
    });

    // Download Individual Event
    $(".download-btn").on("click", function () {
        var eventName = $(this).closest(".timeline-item").find("summary").text().trim();
        var eventData = $(this).closest(".timeline-item").find(".event-content").text().trim();
        var docContent = "<h2>" + eventName + "</h2><p>" + eventData.replace(/\n/g, "<br>") + "</p>";

        var blob = new Blob(["<html><body>" + docContent + "</body></html>"], { type: "application/msword" });
        var a = document.createElement("a");
        a.href = URL.createObjectURL(blob);
        a.download = eventName.replace(/[^a-zA-Z0-9]/g, "_") + ".doc"; // Clean file name
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    });

    // Download All Events
    $("#downloadAllBtn").on("click", function () {
        var docContent = "<h2>Event Timeline</h2>";
        $(".timeline-item").each(function () {
            var eventName = $(this).find("summary").text().trim();
            var eventData = $(this).find(".event-content").text().trim();
            docContent += "<h3>" + eventName + "</h3><p>" + eventData.replace(/\n/g, "<br>") + "</p>";
        });

        var blob = new Blob(["<html><body>" + docContent + "</body></html>"], { type: "application/msword" });
        var a = document.createElement("a");
        a.href = URL.createObjectURL(blob);
        a.download = "EventTimeline.doc";
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    });
});


@{
    var groupedEvents = Model.BusEvents
        .OrderByDescending(be => be.SentDate)
        .GroupBy(be => be.SentDate.Date); // Grouping by date (ignoring time)
}

<div class="timeline">
    @foreach (var group in groupedEvents)
    {
        <div class="time-label">
            <span class="bg-gray">@group.Key.ToString("dd MMM yyyy")</span>
        </div>

        @foreach (var busEvent in group)
        {
            <div class="timeline-item">
                <!-- Kafka notification icon -->
                <i class="fas fa-stream bg-orange"></i> 

                <span class="time">
                    <i class="fas fa-clock"></i> @busEvent.SentDate.ToShortTimeString()
                </span>

                <details>
                    <summary class="card-header">
                        <i class="fas fa-bell text-warning"></i> @busEvent.Name
                    </summary>

                    <div class="card-body p-1" style="max-height:400px; overflow:auto;">
                        <pre class="event-content">@busEvent.EventData</pre>
                    </div>
                </details>

                <!-- Copy & Download Buttons for Each Event -->
                <div class="mt-2">
                    <button class="btn btn-sm btn-primary copy-btn" data-event="@busEvent.Name">
                        <i class="fas fa-copy"></i> Copy
                    </button>
                    <button class="btn btn-sm btn-success download-btn" data-event="@busEvent.Name">
                        <i class="fas fa-file-word"></i> Download
                    </button>
                </div>
            </div>
        }
    }
</div>

<!-- Buttons for Copy All and Download All -->
<div class="mt-3">
    <button class="btn btn-primary" id="copyAllBtn">
        <i class="fas fa-copy"></i> Copy All Events
    </button>
    <button class="btn btn-success" id="downloadAllBtn">
        <i class="fas fa-file-word"></i> Download All Events
    </button>
</div>


$(document).ready(function () {
    var table = $('#PendingAuthsDT').DataTable();

    function filterByDate(startDate, endDate) {
        $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
            var date = new Date(data[1]); // Assuming "Staged Date" is in column index 1
            var min = new Date(startDate);
            var max = new Date(endDate);

            return (date >= min && date <= max);
        });

        table.draw();
    }

    $("#dateRange").daterangepicker({
        autoUpdateInput: false,
        locale: { cancelLabel: 'Clear' },
        ranges: {
            'Today': [moment(), moment()],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 1 Month': [moment().subtract(1, 'months'), moment()],
            'Last 2 Months': [moment().subtract(2, 'months'), moment()],
            'Last 1 Year': [moment().subtract(1, 'years'), moment()],
            'Custom Range': []
        }
    });

    // Apply the selected date range filter
    $("#dateRange").on('apply.daterangepicker', function (ev, picker) {
        var startDate = picker.startDate.format('YYYY-MM-DD');
        var endDate = picker.endDate.format('YYYY-MM-DD');
        $(this).val(startDate + ' to ' + endDate);

        // Remove old filter and apply new
        $.fn.dataTable.ext.search.pop();
        filterByDate(startDate, endDate);
    });

    // Clear filter on cancel
    $("#dateRange").on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $.fn.dataTable.ext.search.pop();
        table.draw();
    });

    // Text search for other columns
    $('#PendingAuthsDT thead').on('keyup', 'input[type="text"]', function () {
        table.column($(this).parent().index())
            .search(this.value)
            .draw();
    });
});


<thead>
    <tr>
        <th><input name="ExportProcess" type="text" class="form-control form-control-sm"></th>
        <th>
            <input type="text" id="dateRange" class="form-control form-control-sm" placeholder="Select Date Range">
        </th>
        <th><input name="AuthID" type="text" class="form-control form-control-sm" placeholder="Auth ID"></th>
        <th><input name="AuthKey" class="form-control form-control-sm" type="text" placeholder="Auth Key"></th>
    </tr>
    <tr class="serp-row-header bg-navy color-palette">
        <th>Export Process</th>
        <th>Staged Date</th>
        <th>Auth ID</th>
        <th>Auth Key</th>
    </tr> 
</thead>

$(document).ready(function () {
    var table = $('#PendingAuthsDT').DataTable();

    // Initialize Date Range Picker
    $("#dateRange").daterangepicker({
        autoUpdateInput: false,
        locale: {
            cancelLabel: 'Clear'
        }
    });

    // Apply the selected date range filter
    $("#dateRange").on('apply.daterangepicker', function (ev, picker) {
        var startDate = picker.startDate.format('YYYY-MM-DD');
        var endDate = picker.endDate.format('YYYY-MM-DD');
        $(this).val(startDate + ' to ' + endDate);

        $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
            var date = new Date(data[1]); // Assuming "Staged Date" is in column index 1
            var min = new Date(startDate);
            var max = new Date(endDate);

            return (date >= min && date <= max);
        });

        table.draw();
    });

    // Clear filter on cancel
    $("#dateRange").on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $.fn.dataTable.ext.search.pop();
        table.draw();
    });

    // Text search for other columns
    $('#PendingAuthsDT thead').on('keyup', 'input[type="text"]', function () {
        table.column($(this).parent().index())
            .search(this.value)
            .draw();
    });
});


<thead>
    <tr>
        <th><input name="ExportProcess" type="text" class="form-control form-control-sm"></th>
        <th>
            <input type="date" id="minDate" class="form-control form-control-sm" placeholder="From Date">
            <input type="date" id="maxDate" class="form-control form-control-sm" placeholder="To Date">
        </th>
        <th><input name="AuthID" type="text" class="form-control form-control-sm" placeholder="Auth ID"></th>
        <th><input name="AuthKey" class="form-control form-control-sm" type="text" placeholder="Auth Key"></th>
    </tr>
    <tr class="serp-row-header bg-navy color-palette">
        <th>Export Process</th>
        <th>Staged Date</th>
        <th>Auth ID</th>
        <th>Auth Key</th>
    </tr> 
</thead>
$(document).ready(function () {
    var table = $('#PendingAuthsDT').DataTable();

    // Custom search function for date range
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        var min = $('#minDate').val() ? new Date($('#minDate').val()) : null;
        var max = $('#maxDate').val() ? new Date($('#maxDate').val()) : null;
        var date = new Date(data[1]); // Assuming "Staged Date" is the second column (index 1)

        if (
            (min === null && max === null) ||
            (min === null && date <= max) ||
            (min <= date && max === null) ||
            (min <= date && date <= max)
        ) {
            return true;
        }
        return false;
    });

    // Trigger filtering when date inputs change
    $('#minDate, #maxDate').on('change', function () {
        table.draw();
    });

    // Text search for other columns
    $('#PendingAuthsDT thead').on('keyup', 'input[type="text"]', function () {
        table.column($(this).parent().index())
            .search(this.value)
            .draw();
    });
});

#contextMenuPendingAuth {
    position: absolute;
    background: white;
    border: 1px solid #ccc;
    box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2);
    padding: 5px 0;
    list-style: none;
    display: none;
    z-index: 1000;
    border-radius: 4px;
    min-width: 150px;
}

/* Arrow pointing to the clicked row */
#contextMenuPendingAuth::before {
    content: "";
    position: absolute;
    top: -10px; /* Adjust based on placement */
    left: 10px; /* Adjust based on desired arrow position */
    width: 0;
    height: 0;
    border-left: 6px solid transparent;
    border-right: 6px solid transparent;
    border-bottom: 10px solid #ccc; /* Arrow color */
}

/* Inner arrow to match menu background */
#contextMenuPendingAuth::after {
    content: "";
    position: absolute;
    top: -8px;
    left: 11px;
    width: 0;
    height: 0;
    border-left: 5px solid transparent;
    border-right: 5px solid transparent;
    border-bottom: 8px solid white; /* Match the background */
}



<section class="content">
    <div class="col-sm-12 col-md-12 ml-3 mb-2">
        <div class="d-flex w-100">
            <div class="col-md-6 input-group mb-1">
                <input class="form-control col-sm-6 input-sm" id="authId" name="authId" placeholder="Enter Authorization">

                <span class="input-group-append">
                    <button type="button" class="btn btn-info btn-block" id="searchBtn">Search</button>
                </span>
            </div>

            <!-- Button aligned to the right -->
            <div class="ml-auto">
                <button type="button" id="compare-export" class="btn btn-warning btn-block">See Export Data</button>
            </div>
        </div>
    </div>

    <!-- Placeholder for AJAX Response -->
    <div id="authHistoryContainer">
        @await Html.PartialAsync("_AuthHistoryPartial.cshtml", Model)
    </div>
</section>

<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    $(document).ready(function () {
        $("#searchBtn").click(function () {
            var authId = $("#authId").val();

            $.ajax({
                url: '@Url.Action("AuthHistory", "Insight")', // Your controller action
                type: "GET", // Change to POST if needed
                data: { authId: authId },
                beforeSend: function () {
                    $("#searchBtn").prop("disabled", true).text("Searching...");
                },
                success: function (response) {
                    $("#authHistoryContainer").html(response);
                },
                complete: function () {
                    $("#searchBtn").prop("disabled", false).text("Search");
                },
                error: function () {
                    alert("An error occurred while fetching data.");
                }
            });
        });
    });
</script>


CREATE PROCEDURE GetExportProcessDetails
    @exportProcessName VARCHAR(255),
    @startDate DATE,
    @endDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporary table to store Authorization Keys with 'Exported' status
    CREATE TABLE #TempAuthorizationDest (AuthorizationKey UNIQUEIDENTIFIER);

    INSERT INTO #TempAuthorizationDest (AuthorizationKey)
    SELECT AD.AuthorizationKey
    FROM Exports.Canonical.AuthorizationDestination AD (NOLOCK)
    INNER JOIN Exports.Canonical.AuthorizationDestinationStatus ADS (NOLOCK)
        ON AD.AuthorizationDestinationStatusKey = ADS.AuthorizationDestinationStatusKey
    WHERE ADS.[Name] = 'Exported';

    -- Common Table Expression (CTE) for Export Process filtering
    WITH cte_ExportProcess AS
    (
        SELECT EP.ExportProcessKey, EP.[Name], EP.ExportProcessDescription
        FROM Exports.Canonical.ExportProcess EP (NOLOCK)
        WHERE @exportProcessName = 'All' OR EP.[Name] = @exportProcessName
    )
    -- Main query to retrieve export batch details
    SELECT 
        EB.CreatedDate,
        EP.[Name] AS ExportProcessName,
        EB.ExportBatchKey,
        COUNT(EB_AI.AuthorizationInstanceKey) AS AuthCount,
        COALESCE(NULLIF(EP.ExportProcessDescription, ''), EP.[Name]) AS ExportProcessDescription
    FROM Exports.EDI.ExportBatch EB (NOLOCK)
    INNER JOIN cte_ExportProcess EP (NOLOCK)
        ON EP.ExportProcessKey = EB.ExportProcessKey
    INNER JOIN Exports.EDI.ExportBatch_AuthorizationInstance EB_AI (NOLOCK)
        ON EB_AI.ExportBatchKey = EB.ExportBatchKey
    INNER JOIN #TempAuthorizationDest TAD (NOLOCK)
        ON TAD.AuthorizationKey = EB_AI.AuthorizationInstanceKey
    WHERE EB.CreatedDate BETWEEN @startDate AND @endDate
    GROUP BY EB.CreatedDate, EB.ExportBatchKey, EP.[Name], EP.ExportProcessDescription
    ORDER BY EB.CreatedDate DESC;

    -- Drop the temporary table
    DROP TABLE #TempAuthorizationDest;
END;


CREATE PROCEDURE GetExportProcessDetails
    @exportProcessName VARCHAR(255),
    @startDate DATE,
    @endDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporary table to store Authorization Keys with 'Exported' status
    CREATE TABLE #TempAuthorizationDest (AuthorizationKey UNIQUEIDENTIFIER);

    INSERT INTO #TempAuthorizationDest (AuthorizationKey)
    SELECT AD.AuthorizationKey
    FROM Exports.Canonical.AuthorizationDestination AD (NOLOCK)
    INNER JOIN Exports.Canonical.AuthorizationDestinationStatus ADS (NOLOCK)
        ON AD.AuthorizationDestinationStatusKey = ADS.AuthorizationDestinationStatusKey
    WHERE ADS.[Name] = 'Exported';

    -- Common Table Expression (CTE) for Export Process filtering
    WITH cte_ExportProcess AS
    (
        SELECT EP.ExportProcessKey, EP.[Name], EP.ExportProcessDescription
        FROM Exports.Canonical.ExportProcess EP (NOLOCK)
        WHERE @exportProcessName = 'All' OR EP.[Name] = @exportProcessName
    )
    -- Main query to retrieve export batch details
    SELECT 
        EB.CreatedDate,
        EP.[Name] AS ExportProcessName,
        EB.ExportBatchKey,
        COUNT(EB_AI.AuthorizationInstanceKey) AS AuthCount,
        ISNULL(NULLIF(EP.ExportProcessDescription, ''), EP.[Name]) AS ExportProcessDescription
    FROM Exports.EDI.ExportBatch EB (NOLOCK)
    INNER JOIN cte_ExportProcess EP (NOLOCK)
        ON EP.ExportProcessKey = EB.ExportProcessKey
    INNER JOIN Exports.EDI.ExportBatch_AuthorizationInstance EB_AI (NOLOCK)
        ON EB_AI.ExportBatchKey = EB.ExportBatchKey
    INNER JOIN #TempAuthorizationDest TAD (NOLOCK)
        ON TAD.AuthorizationKey = EB_AI.AuthorizationInstanceKey
    WHERE EB.CreatedDate BETWEEN @startDate AND @endDate
    GROUP BY EB.CreatedDate, EB.ExportBatchKey, EP.[Name], EP.ExportProcessDescription
    ORDER BY EB.CreatedDate DESC;

    -- Drop the temporary table
    DROP TABLE #TempAuthorizationDest;
END;



WITH cte_ExportProcess AS (
    SELECT EP.ExportProcessKey, EP.[Name], EP.ExportProcessDescription
    FROM Exports.Canonical.ExportProcess EP (NOLOCK)
    WHERE (@exportProcessName = 'All' OR EP.[Name] = @exportProcessName)
      AND (EP.[Name] LIKE '%Batch%' OR EP.[Name] LIKE '%Manual%')
)
SELECT 
    EB.CreatedDate,
    EP.[Name],
    EB.ExportBatchKey,
    COUNT(EB_AI.AuthorizationInstanceKey) AS AuthCount,
    ISNULL(NULLIF(EP.ExportProcessDescription, ''), EP.[Name]) AS ExportProcessDescription
FROM Exports.EDI.ExportBatch EB (NOLOCK) 

-- Use Indexed Joins
INNER LOOP JOIN cte_ExportProcess EP (NOLOCK)
    ON EP.ExportProcessKey = EB.ExportProcessKey

INNER LOOP JOIN Exports.EDI.ExportBatch_AuthorizationInstance EB_AI (NOLOCK)
    ON EB_AI.ExportBatchKey = EB.ExportBatchKey

INNER LOOP JOIN Exports.Canonical.AuthorizationDestination AD (NOLOCK)
    ON AD.AuthorizationKey = EB_AI.AuthorizationInstanceKey

INNER LOOP JOIN Exports.Canonical.AuthorizationDestinationStatus ADS (NOLOCK)
    ON ADS.AuthorizationDestinationStatusKey = AD.AuthorizationDestinationStatusKey

-- Optimized WHERE Condition
WHERE EB.CreatedDate >= @startDate
  AND EB.CreatedDate < DATEADD(DAY, 1, @endDate)

-- Use Hash Aggregation for Faster Grouping
GROUP BY 
    EB.CreatedDate,
    EB.ExportBatchKey,
    EP.[Name],
    EP.ExportProcessDescription
ORDER BY 
    EB.CreatedDate DESC 
OPTION (MAXDOP 4, RECOMPILE);  -- Parallelism and Recompilation for Faster Execution

-- Step 1: Pre-Aggregate Authorization Counts (Reduces Join Complexity)
SELECT 
    EB_AI.ExportBatchKey,
    COUNT(AD.AuthorizationKey) AS AuthCount
INTO #TempAuthCounts
FROM Exports.EDI.ExportBatch_AuthorizationInstance EB_AI (NOLOCK)
INNER JOIN Exports.Canonical.AuthorizationDestination AD (NOLOCK)
    ON AD.AuthorizationKey = EB_AI.AuthorizationInstanceKey
GROUP BY EB_AI.ExportBatchKey;

-- Step 2: Create Index for Faster Lookups
CREATE CLUSTERED INDEX IDX_TempAuthCounts ON #TempAuthCounts (ExportBatchKey);

-- Step 3: Store Export Process Data in a Temporary Table
SELECT ExportProcessKey, [Name], ExportProcessDescription
INTO #Temp_ExportProcess
FROM Exports.Canonical.ExportProcess EP (NOLOCK)
WHERE (@exportProcessName = 'All' OR EP.[Name] = @exportProcessName)
  AND (EP.[Name] LIKE '%Batch%' OR EP.[Name] LIKE 'Manual%');

-- Step 4: Create an Index on the Temporary Export Process Table
CREATE CLUSTERED INDEX IDX_Temp_ExportProcess ON #Temp_ExportProcess (ExportProcessKey);

-- Step 5: Optimized Query with Pre-Aggregation
SELECT 
    EB.CreatedDate,
    EP.[Name],
    EB.ExportBatchKey,
    COALESCE(TAC.AuthCount, 0) AS AuthCount, -- Using Pre-Aggregated Data
    CASE 
        WHEN EP.ExportProcessDescription IS NULL OR EP.ExportProcessDescription = '' 
        THEN EP.[Name]
        ELSE EP.ExportProcessDescription
    END AS ExportProcessDescription
FROM Exports.EDI.ExportBatch EB WITH (INDEX = IDX_ExportBatch_CreatedDate) -- Force Index for Faster Lookup

-- Join with Pre-Filtered Export Process Table
INNER JOIN #Temp_ExportProcess EP (NOLOCK)
    ON EP.ExportProcessKey = EB.ExportProcessKey

-- Join with Pre-Aggregated Authorization Data
LEFT JOIN #TempAuthCounts TAC (NOLOCK)
    ON TAC.ExportBatchKey = EB.ExportBatchKey

-- Corrected WHERE Clause (Avoids Function Calls)
WHERE EB.CreatedDate >= @startDate
  AND EB.CreatedDate < DATEADD(day, 1, @endDate) 

-- Grouping & Sorting
ORDER BY EB.CreatedDate DESC;

-- Step 6: Cleanup Temporary Tables After Query Execution
DROP TABLE #Temp_ExportProcess;
DROP TABLE #TempAuthCounts;


-- Step 1: Pre-Aggregate Authorization Counts
SELECT 
    AuthorizationKey, 
    COUNT(*) AS AuthCount
INTO #TempAuthCounts
FROM Exports.Canonical.AuthorizationDestination (NOLOCK)
GROUP BY AuthorizationKey;

-- Step 2: Create an Index on the Temporary Table
CREATE CLUSTERED INDEX IDX_TempAuthCounts ON #TempAuthCounts (AuthorizationKey);

-- Step 3: Create and Populate Temporary Table Instead of CTE
SELECT *
INTO #TempExportProcess
FROM Exports.Canonical.ExportProcess EP (NOLOCK)
WHERE (@exportProcessName = 'All' OR EP.[Name] = @exportProcessName)
  AND (EP.[Name] LIKE '%Batch%' OR EP.[Name] LIKE 'Manual%');

-- Step 4: Create an Index on the Temporary Table
CREATE CLUSTERED INDEX IDX_TempExportProcess ON #TempExportProcess (ExportProcessKey);

-- Step 5: Optimized Main Query with Pre-Aggregation and Indexing
SELECT 
    EB.CreatedDate,
    EP.[Name],
    EB.ExportBatchKey,
    COALESCE(TAC.AuthCount, 0) AS AuthCount, -- Using Pre-Aggregated Data
    CASE 
        WHEN EP.ExportProcessDescription IS NULL OR EP.ExportProcessDescription = '' 
        THEN EP.[Name]
        ELSE EP.ExportProcessDescription
    END AS ExportProcessDescription
FROM Exports.EDI.ExportBatch EB WITH (INDEX = IDX_ExportBatch_CreatedDate) -- Force Index for Faster Lookup
INNER JOIN #TempExportProcess EP (NOLOCK)
    ON EP.ExportProcessKey = EB.ExportProcessKey
LEFT JOIN #TempAuthCounts TAC (NOLOCK) -- Use Pre-Aggregated Table
    ON TAC.AuthorizationKey = EB.ExportProcessKey
WHERE EB.CreatedDate >= @startDate 
  AND EB.CreatedDate < @endDate + 1 -- Avoids function call in WHERE clause
ORDER BY 
    EB.CreatedDate DESC;

-- Step 6: Drop Temporary Tables After Query Execution
DROP TABLE #TempExportProcess;
DROP TABLE #TempAuthCounts;


-- Step 1: Create and Populate Temporary Table Instead of CTE
SELECT *
INTO #TempExportProcess
FROM Exports.Canonical.ExportProcess EP (NOLOCK)
WHERE (@exportProcessName = 'All' OR EP.[Name] = @exportProcessName)
  AND (EP.[Name] LIKE '%Batch%' OR EP.[Name] LIKE 'Manual%');

-- Step 2: Create an Index on the Temporary Table
CREATE CLUSTERED INDEX IDX_TempExportProcess ON #TempExportProcess (ExportProcessKey);

-- Step 3: Optimized Main Query
SELECT 
    EB.CreatedDate,
    EP.[Name],
    EB.ExportBatchKey,
    COUNT(EB_AI.AuthorizationInstanceKey) AS AuthCount,
    CASE 
        WHEN EP.ExportProcessDescription IS NULL OR EP.ExportProcessDescription = '' 
        THEN EP.[Name]
        ELSE EP.ExportProcessDescription
    END AS ExportProcessDescription
FROM Exports.EDI.ExportBatch EB (NOLOCK)
INNER JOIN #TempExportProcess EP (NOLOCK)
    ON EP.ExportProcessKey = EB.ExportProcessKey
INNER JOIN Exports.Canonical.AuthorizationDestination AD (NOLOCK)
    ON AD.AuthorizationKey = EB_AI.AuthorizationInstanceKey
INNER JOIN Exports.Canonical.AuthorizationDestinationStatus ADS (NOLOCK)
    ON ADS.AuthorizationDestinationStatusKey = AD.AuthorizationDestinationStatusKey
WHERE EB.CreatedDate >= @startDate 
  AND EB.CreatedDate < @endDate + 1 -- Avoids function call in WHERE clause
GROUP BY 
    EB.CreatedDate, 
    EB.ExportBatchKey, 
    EP.[Name], 
    EP.ExportProcessDescription
ORDER BY 
    EB.CreatedDate DESC;

-- Step 4: Drop Temporary Table
DROP TABLE #TempExportProcess;

-- Materialize the CTE into a temporary table for better performance
SELECT 
    EP.ExportProcessKey,
    EP.[Name],
    EP.ExportProcessDescription
INTO #Temp_ExportProcess
FROM Exports.Canonical.ExportProcess EP (NOLOCK)
WHERE (@exportProcessName = 'All' OR EP.[Name] = @exportProcessName)
  AND (EP.[Name] LIKE '%Batch%' OR EP.[Name] LIKE 'Manual%');

-- Main query
SELECT 
    EB.CreatedDate,
    EP.[Name],
    EB.ExportBatchKey,
    COUNT(EB_AI.AuthorizationInstanceKey) AS 'AuthCount',
    CASE 
        WHEN EP.ExportProcessDescription IS NULL OR EP.ExportProcessDescription = '' 
        THEN EP.[Name]
        ELSE EP.ExportProcessDescription
    END AS ExportProcessDescription
FROM Exports.EDI.ExportBatch EB (NOLOCK)
INNER JOIN #Temp_ExportProcess EP
    ON EP.ExportProcessKey = EB.ExportProcessKey
INNER JOIN Exports.Canonical.AuthorizationDestination AD (NOLOCK)
    ON AD.AuthorizationKey = EB_AI.AuthorizationInstanceKey
INNER JOIN Exports.Canonical.AuthorizationDestinationStatus ADS (NOLOCK)
    ON ADS.AuthorizationDestinationStatusKey = AD.AuthorizationDestinationStatusKey
WHERE EB.CreatedDate >= @startDate 
  AND EB.CreatedDate < DATEADD(day, 1, @endDate)
GROUP BY 
    EB.CreatedDate, 
    EB.ExportBatchKey, 
    EP.[Name], 
    EP.ExportProcessDescription
ORDER BY 
    EB.CreatedDate DESC;

-- Clean up temporary table
DROP TABLE #Temp_ExportProcess;


WITH cte_ExportProcess AS (
    SELECT *
    FROM Exports.Canonical.ExportProcess EP (NOLOCK)
    WHERE (@exportProcessName = 'All' OR EP.[Name] = @exportProcessName)
      AND (EP.[Name] LIKE '%Batch%' OR EP.[Name] LIKE 'Manual%')
)
SELECT 
    EB.CreatedDate,
    EP.[Name],
    EB.ExportBatchKey,
    COUNT(EB_AI.AuthorizationInstanceKey) AS 'AuthCount',
    CASE 
        WHEN EP.ExportProcessDescription IS NULL OR EP.ExportProcessDescription = '' 
        THEN EP.[Name]
        ELSE EP.ExportProcessDescription
    END AS ExportProcessDescription
FROM Exports.EDI.ExportBatch EB (NOLOCK)
INNER JOIN cte_ExportProcess EP (NOLOCK)
    ON EP.ExportProcessKey = EB.ExportProcessKey
INNER JOIN Exports.Canonical.AuthorizationDestination AD (NOLOCK)
    ON AD.AuthorizationKey = EB_AI.AuthorizationInstanceKey
INNER JOIN Exports.Canonical.AuthorizationDestinationStatus ADS (NOLOCK)
    ON ADS.AuthorizationDestinationStatusKey = AD.AuthorizationDestinationStatusKey
WHERE EB.CreatedDate >= @startDate 
  AND EB.CreatedDate < DATEADD(day, 1, @endDate)
GROUP BY 
    EB.CreatedDate, 
    EB.ExportBatchKey, 
    EP.[Name], 
    EP.ExportProcessDescription
ORDER BY 
    EB.CreatedDate DESC;

var table = $('#exportProcessDT').DataTable({
    serverSide: true,
    processing: true,
    ajax: {
        url: '/Insight/GetBatchHistoryData',
        type: 'POST',
        data: function (d) {
            d.exportProcessName = $('#exportProcessName').val();
            d.daterange = $('#daterange').val();
            d.sortColumn = d.order.length > 0 ? d.columns[d.order[0].column].data : "createdDate";
            d.sortDirection = d.order.length > 0 ? d.order[0].dir : "desc";
            
            // 🔹 Pass column-wise search values correctly
            d.columns.forEach((col, index) => {
                d.columns[index].search = { value: $('#exportProcessDT thead input').eq(index).val() || "" };
            });
        }
    },
    columns: [
        { data: "batchKey", visible: false }, 
        { data: "exportProcessDescription" },
        { data: "exportProcessName" },
        { data: "createdDate" },
        { data: "authorizationIncluded" }
    ],
    order: [[3, "desc"]],
    lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
    dom: "<'row'<'col-sm-5'B><'col-sm-3'l><'col-sm-4'f>>" +
         "<'row'<'col-sm-12'tr>>" +
         "<'row'<'col-sm-6'i><'col-sm-6'p>>",
});

// 🔹 Add Column-Wise Search with Debounce
$('#exportProcessDT thead input').on('keyup', function () {
    var $input = $(this);
    clearTimeout($input.data('timeout'));
    
    $input.data('timeout', setTimeout(function () {
        table.column($input.closest('th').index()).search($input.val()).draw();
    }, 500)); // Delay of 500ms to prevent excessive calls
});

// 🔹 Reload Data on Button Click
$('#batchHistoryButton').on('click', function (e) {
    e.preventDefault();
    table.ajax.reload();
});


// 🔹 Ensure sortColumn is mapped correctly
if (!string.IsNullOrEmpty(sortColumn) && columnMap.ContainsKey(sortColumn))
{
    sortColumn = columnMap[sortColumn];

    var parameter = Expression.Parameter(typeof(BatchHistoryDataModel), "x");
    var property = Expression.Property(parameter, sortColumn);
    var lambda = Expression.Lambda(property, parameter);

    var methodName = sortDirection.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";
    var orderByMethod = typeof(Queryable).GetMethods()
        .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
        .Single()
        .MakeGenericMethod(typeof(BatchHistoryDataModel), property.Type);

    query = (IQueryable<BatchHistoryDataModel>)orderByMethod.Invoke(null, new object[] { query, lambda });
}

[HttpPost]
public IActionResult GetBatchHistoryData(string exportProcessName, string daterange, int draw, int start, int length, string searchValue, string sortColumn, string sortDirection)
{
    DateTime startDate, endDate;

    if (string.IsNullOrEmpty(daterange))
    {
        DateTime currentDate = DateTime.Now;
        startDate = currentDate.AddDays(-4);
        endDate = currentDate;
    }
    else
    {
        string[] dateValues = daterange.Split('-').Select(s => s.Trim()).ToArray();
        startDate = Convert.ToDateTime(dateValues[0]);
        endDate = Convert.ToDateTime(dateValues[1]);
    }

    var query = _batchHistory.GetBatchHistoryData(exportProcessName, startDate, endDate).AsQueryable();

    // 🔹 Apply Search Filter
    if (!string.IsNullOrEmpty(searchValue))
    {
        query = query.Where(x =>
            x.Name.Contains(searchValue) ||
            x.ExportProcessDescription.Contains(searchValue) ||
            x.AuthCount.ToString().Contains(searchValue)
        );
    }

    // 🔹 Convert lowercase JS sortColumn to C# property names
    Dictionary<string, string> columnMap = new Dictionary<string, string>
    {
        { "batchKey", "ExportBatchKey" },
        { "exportProcessDescription", "ExportProcessDescription" },
        { "exportProcessName", "Name" },
        { "createdDate", "CreatedDate" }, // 🔹 Ensure correct mapping
        { "authorizationIncluded", "AuthCount" }
    };

    // 🔹 Ensure sortColumn is mapped correctly
    if (!string.IsNullOrEmpty(sortColumn) && columnMap.ContainsKey(sortColumn))
    {
        sortColumn = columnMap[sortColumn];

        query = sortDirection.ToLower() == "asc"
            ? query.OrderBy(x => EF.Property<object>(x, sortColumn))
            : query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
    }

    int totalRecords = query.Count();

    var batchCounts = query
        .Skip(start)
        .Take(length)
        .Select(batch => new
        {
            batchKey = batch.ExportBatchKey.ToString(),
            exportProcessDescription = batch.ExportProcessDescription,
            exportProcessName = batch.Name,
            createdDate = batch.CreatedDate, // 🔹 Ensure correct mapping
            authorizationIncluded = batch.AuthCount
        })
        .ToList();

    return Json(new
    {
        draw = draw,
        recordsTotal = totalRecords,
        recordsFiltered = totalRecords,
        data = batchCounts
    });
}


public class DataTableRequestModel
{
    public int draw { get; set; } // Unique request ID
    public int start { get; set; } // Paging start index
    public int length { get; set; } // Number of records per page
    public List<DataTableColumn> columns { get; set; } // Column definitions
    public List<DataTableOrder> order { get; set; } // Sorting information
    public DataTableSearch search { get; set; } // Global search
    public string exportProcessName { get; set; } // Custom parameter
    public DateTime startDate { get; set; } // Custom parameter
    public DateTime endDate { get; set; } // Custom parameter
}

public class DataTableColumn
{
    public string data { get; set; } // Column name
    public string name { get; set; } // Column alias
    public bool searchable { get; set; } // Can be searched
    public bool orderable { get; set; } // Can be sorted
    public DataTableSearch search { get; set; } // Search data
}

public class DataTableOrder
{
    public int column { get; set; } // Column index
    public string dir { get; set; } // Sort direction (asc/desc)
}

public class DataTableSearch
{
    public string value { get; set; } // Search value
    public bool regex { get; set; } // Regex search
}

[HttpPost]
public IActionResult GetBatchHistoryData([FromBody] DataTableRequestModel request)
{
    IQueryable<BatchHistoryDataModel> query = _batchHistory.GetBatchHistoryData(request.exportProcessName, request.startDate, request.endDate);

    // Apply column-wise search filters
    if (!string.IsNullOrEmpty(request.columns[1].search.value))
        query = query.Where(x => x.ExportProcessDescription.Contains(request.columns[1].search.value));
    
    if (!string.IsNullOrEmpty(request.columns[2].search.value))
        query = query.Where(x => x.ExportProcessName.Contains(request.columns[2].search.value));

    if (!string.IsNullOrEmpty(request.columns[3].search.value))
        query = query.Where(x => x.CreatedDate.ToString().Contains(request.columns[3].search.value));

    if (!string.IsNullOrEmpty(request.columns[4].search.value))
        query = query.Where(x => x.AuthorizationIncluded.ToString().Contains(request.columns[4].search.value));

    int totalRecords = query.Count();
    var batchHistory = query.Skip(request.start).Take(request.length).ToList();

    return Json(new { draw = request.draw, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = batchHistory });
}


$(document).ready(function () {
    var table = $('#exportProcessDT').DataTable({
        processing: true,
        serverSide: true,
        searching: true,
        ordering: true,
        paging: true,
        ajax: {
            url: '/Insight/GetBatchHistoryData',
            type: 'POST',
            data: function (d) {
                var columnMap = {
                    0: "batchKey",
                    1: "exportProcessDescription",
                    2: "exportProcessName",
                    3: "createdDate",
                    4: "authorizationIncluded"
                };

                var orderColumnIndex = d.order.length > 0 ? d.order[0].column : 3;
                d.sortColumn = columnMap[orderColumnIndex] || "createdDate";
                d.sortDirection = d.order.length > 0 ? d.order[0].dir : "desc";

                // Add column-wise search values
                $('#exportProcessDT thead .column-search').each(function () {
                    var index = $(this).data('index');
                    d['columns[' + index + '][search][value]'] = $(this).val();
                });
            }
        },
        columns: [
            { data: "batchKey", visible: false },
            { data: "exportProcessDescription" },
            { data: "exportProcessName" },
            { data: "createdDate" },
            { data: "authorizationIncluded" }
        ]
    });

    // 🔹 Enable Column-wise Search
    $('#exportProcessDT thead .column-search').on('keyup change', function () {
        table.draw();  // Refresh DataTable on input change
    });
});


$('#exportProcessDT thead input').on('keyup', function () {
        var columnIndex = $(this).parent().index();
        table.column(columnIndex).search(this.value).draw();
    });


[HttpPost]
public IActionResult GetBatchHistoryData(string exportProcessName, string daterange, int draw, int start, int length, string searchValue, string sortColumn, string sortDirection)
{
    DateTime startDate, endDate;

    if (string.IsNullOrEmpty(daterange))
    {
        DateTime currentDate = DateTime.Now;
        startDate = currentDate.AddDays(-4);
        endDate = currentDate;
    }
    else
    {
        string[] dateValues = daterange.Split('-').Select(s => s.Trim()).ToArray();
        startDate = Convert.ToDateTime(dateValues[0]);
        endDate = Convert.ToDateTime(dateValues[1]);
    }

    var query = _batchHistory.GetBatchHistoryData(exportProcessName, startDate, endDate);

    // 🔹 Apply Search Filter
    if (!string.IsNullOrEmpty(searchValue))
    {
        query = query.Where(x =>
            x.Name.Contains(searchValue) ||
            x.ExportProcessDescription.Contains(searchValue) ||
            x.AuthCount.ToString().Contains(searchValue)
        );
    }

    // 🔹 Apply Sorting
    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
    {
        query = sortDirection.ToLower() == "asc"
            ? query.OrderBy(x => EF.Property<object>(x, sortColumn))
            : query.OrderByDescending(x => EF.Property<object>(x, sortColumn));
    }

    int totalRecords = query.Count();  // ✅ Total before pagination

    var batchCounts = query
        .Skip(start)
        .Take(length)
        .Select(batch => new
        {
            BatchKey = batch.ExportBatchKey.ToString(),
            ExportProcessDescription = batch.ExportProcessDescription,
            ExportProcessName = batch.Name,
            Date = batch.CreatedDate.ToString("yyyy-MM-dd"),
            AuthorizationIncluded = batch.AuthCount
        })
        .ToList();

    return Json(new
    {
        draw = draw,
        recordsTotal = totalRecords,
        recordsFiltered = totalRecords,
        data = batchCounts
    });
}


$(document).ready(function () {
    var table = $('#exportProcessDT').DataTable({
        processing: true,
        serverSide: true,
        searching: true,
        ordering: true,
        paging: true,
        ajax: {
            url: '/Insight/GetBatchHistoryData',
            type: 'POST',
            data: function (d) {
                d.exportProcessName = $('#exportProcessName').val();
                d.daterange = $('#daterange').val();
                d.searchValue = d.search.value;  // 🔹 Pass search text
                d.sortColumn = d.order.length > 0 ? d.columns[d.order[0].column].data : "Date";
                d.sortDirection = d.order.length > 0 ? d.order[0].dir : "desc";
            }
        },
        columns: [
            { data: "BatchKey", visible: false, defaultContent: "" },
            { data: "ExportProcessDescription", defaultContent: "" },
            { data: "ExportProcessName", defaultContent: "" },
            { data: "Date", defaultContent: "" },
            { data: "AuthorizationIncluded", defaultContent: "0" }
        ]
    });

    $('#batchHistoryButton').click(function (e) {
        e.preventDefault();
        table.ajax.reload(); // 🔹 Refresh data without full reload
    });
});



[HttpPost]
public IActionResult GetBatchHistoryData([FromForm] DataTableRequest request)
{
    DateTime startDate;
    DateTime endDate;

    if (string.IsNullOrEmpty(request.daterange))
    {
        DateTime currentDate = DateTime.Now;
        startDate = currentDate.AddDays(-4);
        endDate = currentDate;
    }
    else
    {
        string[] dateValues = request.daterange.Split('-').Select(s => s.Trim()).ToArray();
        startDate = DateTime.ParseExact(dateValues[0], "MM/dd/yyyy", CultureInfo.InvariantCulture);
        endDate = DateTime.ParseExact(dateValues[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);
    }

    var query = _batchHistory.GetBatchHistoryData(request.exportProcessName, startDate.Date, endDate.Date)
        .AsQueryable();

    int totalRecords = query.Count();

    var data = query
        .Skip(request.start)
        .Take(request.length)
        .Select(b => new
        {
            BatchKey = b.ExportBatchKey,
            ExportProcessDescription = b.ExportProcessDescription,
            ExportProcessName = b.Name,
            Date = b.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss tt"), // Formatting Date
            AuthorizationIncluded = b.AuthCount
        })
        .ToList();

    return Json(new
    {
        draw = request.draw,
        recordsTotal = totalRecords,
        recordsFiltered = totalRecords,
        data = data
    });
}




public class DataTableRequest
{
    public int draw { get; set; }
    public int start { get; set; }
    public int length { get; set; }
    public string exportProcessName { get; set; }
    public string daterange { get; set; }
}



public IActionResult EdiBatchHistory(string daterange,string exportProcessName = "All")
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            if (daterange == null)
            {
                DateTime currentDate = DateTime.Now;
                 startDate = currentDate.AddDays(-4);
                 endDate = currentDate;

                string formattedStartDate = startDate.ToString("MM/dd/yyyy");
                string formattedEndDate = endDate.ToString("MM/dd/yyyy");

                 daterange =$"{formattedStartDate}-{formattedEndDate}";
            }
            else
            {
                string[] dateValues = daterange.Split('-').Select(sValue => sValue.Trim()).ToArray();
                startDate = Convert.ToDateTime(dateValues[0]);
                endDate = Convert.ToDateTime(dateValues[1]);
                
            }
            
            SerilogPlugin.logInformation("EdiBatchHistory", "EdiBatchHistory - Method Entry", @object: daterange + "," + exportProcessName);
            ViewData["Title"] = "Batch History";
            EdiBatchInsightModel ediBatchInsightModel = new EdiBatchInsightModel();
            ediBatchInsightModel.batchCounts = new List<BatchCount>();          

            List<BatchHistoryDataModel> batchResult = _batchHistory.GetBatchHistoryData(exportProcessName, startDate.Date,endDate.Date);            
            if (_batchMemoryCache.Get<List<ExportProcessViewModel>>("ExportProcesses") == null)
            {
                List<ExportProcessViewModel> exportProcesses = GetExportProcesses();
                _batchMemoryCache.Set<List<ExportProcessViewModel>>("ExportProcesses",exportProcesses.Where(x=>x.Name.Contains("Batch") || x.Name.Contains("Manual")).ToList());
            }

            ViewBag.exportProcesses = _batchMemoryCache.Get<List<ExportProcessViewModel>>("ExportProcesses");

            List<BatchCount> batchCounts = batchResult.Select(exportBatch =>
             new BatchCount
             {
                 ExportProcessName = exportBatch.Name,
                 ExportDate = exportBatch.CreatedDate,
                 Count = exportBatch.AuthCount,
                 BatchKey = exportBatch.ExportBatchKey,
                 ExportProcessDescription = exportBatch.ExportProcessDescription
             }).ToList();

            ediBatchInsightModel.batchCounts.AddRange(batchCounts);
            ViewBag.exportProcessNameParam = exportProcessName;            
            ViewBag.dateRangeParam = daterange;
            SerilogPlugin.logInformation("EdiBatchHistory", "EdiBatchHistory- Method Exit", @object: ediBatchInsightModel.batchCounts);
            return View(ediBatchInsightModel);
        }
$(document).ready(function () {
    var table = $('#exportProcessDT').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Insight/GetBatchHistoryData', // Update the URL to match your controller action
            type: 'POST',
            data: function (d) {
                d.exportProcessName = $('#exportProcessName').val();
                d.daterange = $('#daterange').val();
            }
        },
        columns: [
            { data: "BatchKey", name: "BatchKey", visible: false }, // Hidden field
            { data: "ExportProcessDescription", name: "ExportProcessDescription" },
            { data: "ExportProcessName", name: "ExportProcessName" },
            { data: "Date", name: "Date" },
            { data: "AuthorizationIncluded", name: "AuthorizationIncluded" }
        ],
        order: [[3, "desc"]], // Sorting by Date column
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom:
            "<'row'<'col-sm-5'B><'col-sm-3'l><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        buttons: [
            {
                extend: 'csv',
                className: 'btn-sm dtbuttons btn-outline-secondary',
                text: 'CSV'
            },
            {
                extend: 'excel',
                className: 'btn-sm dtbuttons btn-outline-secondary',
                text: 'Excel'
            }
        ],
        drawCallback: function () {
            var api = this.api();
            if (api.rows().count() > 0) {
                $('.button-history').show();
            } else {
                $('.button-history').hide();
            }
        }
    });

    $('#batchHistoryButton').on('click', function (e) {
        e.preventDefault();
        table.ajax.reload();
    });
});

<div class="card">
    <ol class="breadcrumb">
        <li id="breadcrumb-main" class="breadcrumb-item">Insight</li>
        <li id="breadcrumb-current" class="breadcrumb-item active">Batch History</li> &nbsp;
        <li class="fas fa-info-circle text-primary breadcrumb tooltipicon pb-2"
            data-toggle="tooltipBatchHistory"
            data-custom-class="custom-tooltip"
            tabindex="0"
            data-html="true"
            data-placement="auto"
            title="@toolTipMsg">
        </li>
    </ol>
    <section class="content">
        <div class="card-body">
            <div id="mainBatchHistory">
                <form id="batchHistoryForm">
                    <div class="form-group input-group input-group-sm row ml-1">
                        <label for="exportProcessName" class="mr-2 mt-1">Export Process:</label>
                        <select class="form-control form-control-sm col-md-3" name="exportProcessName" id="exportProcessName">
                            <option>All</option>
                            @foreach (var item in ViewBag.exportProcesses)
                            {
                                <option value="@item.Name" title="@item.Name">@item.ExportProcessDescription</option>
                            }
                        </select>
                        &nbsp;&nbsp;
                        <label for="daterange" class="mr-2 mt-1">Date Range:</label>
                        <input type="text" id="daterange" name="daterange" class="form-control form-control-sm col-md-2" placeholder="YYYY-MM-DD - YYYY-MM-DD" />
                        <button id="batchHistoryButton" class="btn btn-info searchButton form-control-sm" type="submit">Search</button>
                    </div>
                </form>

                <table id="exportProcessDT" class="table nowrap display table-striped table-bordered table-hover table-sm" width="100%">
                    <thead>
                        <tr>
                            <th hidden>Batch Key</th>
                            <th>Export Process Description</th>
                            <th>Export Process Name</th>
                            <th>Created Date</th>
                            <th>Authorization Included</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </section>
</div>

<div class="card">
    <ol class="breadcrumb">
        <li id="breadcrumb-main" class="breadcrumb-item">Insight</li>
        <li id="breadcrumb-current" class="breadcrumb-item active">Batch History</li> &nbsp;
        <li class="fas fa-info-circle text-primary breadcrumb tooltipicon pb-2"
            data-toggle="tooltipBatchHistory"
            data-custom-class="custom-tooltip"
            tabindex="0"
            data-html="true"
            data-placement="auto"
            title="@toolTipMsg">
        </li>
    </ol>
    <section class="content">
        <div class="card-body">
            <div id="mainBatchHistory">
                <form id="batchHistoryForm">
                    <div class="form-group input-group input-group-sm row ml-1">
                        <label for="exportProcessName" class="mr-2 mt-1">Export Process:</label>
                        <select class="form-control form-control-sm col-md-3" name="exportProcessName" id="exportProcessName">
                            <option>All</option>
                            @foreach (var item in ViewBag.exportProcesses)
                            {
                                <option value="@item.Name" title="@item.Name">@item.ExportProcessDescription</option>
                            }
                        </select>
                        &nbsp;&nbsp;
                        <label for="daterange" class="mr-2 mt-1">Date Range:</label>
                        <input type="text" id="daterange" name="daterange" class="form-control form-control-sm col-md-2" placeholder="YYYY-MM-DD - YYYY-MM-DD" />
                        <button id="batchHistoryButton" class="btn btn-info searchButton form-control-sm" type="submit">Search</button>
                    </div>
                </form>

                <table id="exportProcessDT" class="table nowrap display table-striped table-bordered table-hover table-sm" width="100%">
                    <thead>
                        <tr>
                            <th hidden>Batch Key</th>
                            <th>Export Process Description</th>
                            <th>Export Process Name</th>
                            <th>Created Date</th>
                            <th>Authorization Included</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </section>
</div>

[HttpPost]
public IActionResult GetBatchHistoryData([FromForm] DataTableRequest request)
{
    var query = _context.BatchHistory.AsQueryable(); // Replace _context with your actual DB context

    if (!string.IsNullOrEmpty(request.exportProcessName) && request.exportProcessName != "All")
    {
        query = query.Where(b => b.ExportProcessName == request.exportProcessName);
    }

    if (!string.IsNullOrEmpty(request.daterange))
    {
        var dates = request.daterange.Split('-');
        if (dates.Length == 2)
        {
            DateTime startDate = DateTime.Parse(dates[0].Trim());
            DateTime endDate = DateTime.Parse(dates[1].Trim());
            query = query.Where(b => b.Date >= startDate && b.Date <= endDate);
        }
    }

    int totalRecords = query.Count();

    var data = query
        .Skip(request.start)
        .Take(request.length)
        .Select(b => new
        {
            BatchKey = b.BatchKey,
            ExportProcessDescription = b.ExportProcessDescription,
            ExportProcessName = b.ExportProcessName,
            Date = b.Date.ToString("yyyy-MM-dd hh:mm:ss tt"), // Formatting Date
            AuthorizationIncluded = b.AuthorizationIncluded
        })
        .ToList();

    return Json(new
    {
        draw = request.draw,
        recordsTotal = totalRecords,
        recordsFiltered = totalRecords,
        data = data
    });
                }


<script>
    var dataFromServer = @Html.Raw(Json.Serialize(Model));

</script>
<div class="card">
    <ol class="breadcrumb">
        <li id="breadcrumb-main" class="breadcrumb-item">Insight</li>
        <li id="breadcrumb-current" class="breadcrumb-item active">Batch History</li> &nbsp;
        <li class="fas fa-info-circle text-primary breadcrumb tooltipicon pb-2" data-toggle="tooltipBatchHistory" data-custom-class="custom-tooltip" tabindex="0" data-html="true" data-placement="auto" title="@toolTipMsg"></li>
    </ol>
    <section class="content">
        <div class="card-body" style="margin-top:-8px">            
                <div id="mainBatchHistory">
                    <form asp-controller="Insight" asp-action="EdiBatchHistory">
                        <div class="form-group input-group input-group-sm row ml-1">
                            <label for="exportProcessName" class="mr-2 mt-1">Export Process:</label>
                            <select class="form-control form-control-sm  col-md-3 col-sm-3 col-xl-3 col-lg-3 col-xs-3 ml-1" name="exportProcessName" id="exportProcessName">
                                <Option>All</Option>
                                @foreach (dynamic item in ViewBag.exportProcesses)
                                {
                                    if (item.Name.Equals(ViewBag.exportProcessNameParam))
                                    {
                                        <option value="@item.Name" title="@item.Name" selected="selected">@item.ExportProcessDescription</option>
                                    }
                                    else
                                    {
                                        <option value="@item.Name" title="@item.Name">@item.ExportProcessDescription</option>
                                    }
                                }
                            </select>
                            &nbsp;&nbsp;
                            <label for="daterange" class="mr-2 mt-1">Date Range:</label>
                            <div id="selectDates" title="@ViewBag.daterangeparam" data-toggle="tooltipBatchHistory" tabindex="0" data-html="true" data-placement="auto"
                                 class="selectbox form-control pull-right form-control-sm col-md-2 col-sm-2 col-xl-2 col-lg-2 col-xs-2 mr-3">
                                <i class="fa fa-calendar-alt mr-1"></i>
                                <span id="dateRangeText"></span>
                                <i class="fas fa-caret-down caret mb-1"></i>
                            </div>
                            <input type="hidden" id="daterange" name="daterange" value="@ViewBag.daterangeparam">

                            <button id="batchHistoryButton" class="btn btn-info searchButton form-control-sm" type="submit">Search</button>

                        </div>
                    </form>
                    <table id="exportProcessDT" class="table nowrap display table-striped table-panel table-bordered table-hover table-sm" width="100%" style="width:100%" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr>
                                <th hidden><input name="BatchKey" type="text" class="form-control form-control-sm" placeholder="Batch Key" data-index="0" autocomplete="off" /></th>
                                <th><input autocomplete="off" name="ExportProcessDescription" type="text" class="form-control form-control-sm" placeholder="Export Process Description" data-index="1" /></th>
                                <th><input autocomplete="off" name="ExportProcessName" type="text" class="form-control form-control-sm" placeholder="Export Process Name" data-index="2" /></th>
                                <th><input autocomplete="off" name="Date" class="form-control form-control-sm" type="text" placeholder="Date" data-index="3" /></th>
                                <th><input autocomplete="off" name="AuthorizationIncluded" class="form-control form-control-sm" type="text" placeholder="Authorization Included" data-index="4" /></th>
                            </tr>
                            <tr class="serp-row-header bg-navy color-palette">
                                <th hidden>Batch Key</th>
                                <th>Export Process Description</th>
                                <th>Export Process Name</th>
                                <th>Created Date</th>
                                <th>Authorization Included</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div id="ViewAuthorizationDetails" style="display:none; width:100%"></div>
                <div id="authHistorySection" style="display:none; width:100%"></div>
                <div id="CompareTool" style="display:none; width:100%" class="ml-2"></div>
                <div id="spinner" class="text-center text-primary" style="display:none">
                    <span class="spinner-border spinner-border-sm" role="status"></span>
                    <span>Loading...</span>
                </div>
            </div>        
    </section>
</div>
<div id="contextMenuBatchHistory" class="dropdown-menu" style="display:none;">
    <a id="authInfo" class="dropdown-item" href="javascript:void(0)"><i style="color:mediumpurple" class="fa-solid fa-key mr-1"></i>View AuthID Info</a>
</div>
<div id="contextMenuBatchAuthHistory" class="dropdown-menu" style="display:none;">
    <a id="authhistory" class="dropdown-item" href="javascript:void(0)"><i class="fas fa-landmark nav-icon mr-1" style="color:lightskyblue"></i>Auth History</a>
    <a id="CompareExport" class="dropdown-item" href="javascript:void(0)"><i class="fas fa-code-compare nav-icon mr-1" style="color:#FC2847"></i>Compare Tool</a>
</div>


$(document).ready(function ()
{
    const date = moment().format('MM-DD-yyyy HH:MM:ss');

    $('[data-toggle="tooltipBatchHistory').tooltip({
        trigger: 'hover'
    });

    $(document).on('input paste', 'input[type="text"], input[type="search"]', function (event) {
        let inputField = $(this);
        if (event.type === 'paste') {
            event.preventDefault();

            // Fix: Ensure clipboardData exists before using getData
            let clipboardData = event.originalEvent.clipboardData || window.clipboardData;
            if (clipboardData) {
                let text = clipboardData.getData('text');
                inputField.val(text.replace(/\s+/g, ' ').trim());
            }
        } else {
            inputField.val(inputField.val().replace(/\s+/g, ' ').trim());
        }
    });
   
    //Context menu start 
    $('#exportProcessDT').on('contextmenu', 'td', function (e) {
        e.preventDefault();
        var table = $('#exportProcessDT').DataTable();
        var rowIndex = table.cell($(this)).index().row;
        var columns = table.columns().header().toArray();
        var idColumnIndex = columns.findIndex(function (element) {
            return $(element).text().trim() === 'Batch Key';
        });
        var id = table.cell(rowIndex, idColumnIndex).data();
        
        if (table.data().count() > 0) {

            $('#exportProcessDT').data('row-id', id);
            $('#contextMenuBatchHistory').css({
                display: "block",
                left: e.pageX,
                top: e.pageY
            });
        }

    });

    $(document).on('click', function () {
        $('#contextMenuBatchHistory').hide();
    });

    $('#authInfo').off('click').on('click', function ()
    {
         
        var batchKey = $('#exportProcessDT').data('row-id');

        var msg = "Click here navigate back to the Batch History page";
        $('#breadcrumb-main').html(`<a href="javascript:void(0)" id="backToMain"  data-bs-toggle="tooltip" title="${msg}"><i class="fas fa-reply"></i> Batch History</a>`);
        $('#breadcrumb-current').text('Batch History Auth Details');

        var backtomain = document.getElementById('backToMain');
        var tooltipEle = new bootstrap.Tooltip(backtomain, {
            placement: 'right',
            trigger: 'hover'
        });

        tooltipEle.show();
        setTimeout(function () {
            tooltipEle.hide();
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        }, 1400);
        
        $('#mainBatchHistory').slideUp(function ()
        {          
            GetAuthorizationView(batchKey);            
        });

        $(document).on('click', '#backToMain', function () {
            $('.tooltip').remove();
            $('#ViewAuthorizationDetails').slideUp(function () {
                $('#breadcrumb-main').text('Insight');
                $('#breadcrumb-current').text('Batch History');
                $('#mainBatchHistory').slideDown(function () {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                });
            });
        });

    });

   

    function GetAuthorizationView(batchKey) {  
        $('#spinner').show();
        $.ajax({
            method: 'GET',
            url: '/Insight/GetInternalAuthorizationIdData',
            data: { batchkey: batchKey },
            success: function (response) {                 
                $('#ViewAuthorizationDetails').html(response);                           
                setTimeout(function () {  
                    $('#spinner').hide();
                    $('#ViewAuthorizationDetails').slideDown();                    
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                }, 300);          
               
            },
            error: function () {
                $('#spinner').hide();
                alert("error");
            }
        });
       
    }

    var selectedDaterange = document.getElementById('daterange').value;
    $('#dateRangeText').text(selectedDaterange);
    $('#selectDates').daterangepicker({
        opens: "right", // Open to the right
        autoUpdateInput: false, // Prevent auto-filling the input field
        showDropdowns: true, // Allow selecting years and months
        minYear: 1880,
        maxDate: moment(), // Restrict to the current date
        linkedCalendars: false, // Prevents automatic end date change
        autoApply: false, // Requires explicit selection       
        locale: {
            cancelLabel: 'Clear',
            format: 'MM/DD/YYYY', // Date format
            applyLabel: 'Apply',
            fromLabel: 'From',
            toLabel: 'To',
            separator: ' - '
        },
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    });

    // When a date range is selected
    $('#selectDates').on('apply.daterangepicker', function (ev, picker) {
        var startDate = picker.startDate.format('MM/DD/YYYY');
        var endDate = picker.endDate.format('MM/DD/YYYY');

        $('#dateRangeText').text(startDate + ' - ' + endDate);
        $('#daterange').val(startDate + ' - ' + endDate);
    });

    // Clear selection on cancel
    $('#selectDates').on('cancel.daterangepicker', function (ev, picker) {
        var last5DaysStart = moment().subtract(4, 'days');
        var last5DaysEnd = moment();

        $('#selectDates').data('daterangepicker').setStartDate(last5DaysStart);
        $('#selectDates').data('daterangepicker').setEndDate(last5DaysEnd);

        var startDate = last5DaysStart.format('MM/DD/YYYY');
        var endDate = last5DaysEnd.format('MM/DD/YYYY');

        $('#dateRangeText').text(startDate + ' - ' + endDate);
        $('#daterange').val(startDate + ' - ' + endDate);
    });

   
    $('[data-toggle="tooltipBatchHistory"]').tooltip({
        placement: 'auto'
    });

    $('#exportProcessDT thead').on('keyup', 'input', function () {
        var table = $('#exportProcessDT').DataTable();
        table.column($(this).data('index'))
            .search(this.value)
            .draw();
    });
    let dataSet = [];
    var len = dataFromServer.batchCounts.length;
    dataSet = Array.from(dataFromServer.batchCounts, (item) => [
        item.batchKey.trim(),
        item.exportProcessDescription.trim(),
        item.exportProcessName.trim(),
        moment(item.exportDate).format('yyyy-MM-DD hh:mm:ss A').trim(),
        item.count,
    ]);

    $("#exportProcessDT").DataTable({
        data: dataSet,
        processing: true,                  
        scrollY: '60vh',
        scrollX:true,
        autoWidth: true,
        deferRender: true,
        pageLength: 50,
        scrollCollapse: true,
        searchDelay: 200,
        scroller: true,
        order: [[3, 'desc']],
        stateSave:true,
        responsive:true,
        columnDefs: [
            {
                targets: 0,
                visible: false
            }
        ],
        buttons: [
            {
                extend: 'copyHtml5',
                className: 'btn-sm button-history btn-outline-primary dtbuttons',
                text: '<i class="fa fa-copy"></i>',
                titleAttr: 'Copy',
                exportOptions: {
                    columns: ':visible'
                },
                action: function (e, dt, button, config) {
                    // Custom implementation for faster copying
                    const columnHeaders = dt.columns().header().toArray().map(header => header.innerText);
                    const rowData = dt.rows({ search: 'applied' }).data().toArray().map(row => Object.values(row).join('\t'));
                    const clipboardData = [columnHeaders.join('\t'), ...rowData].join('\n');

                    const tempTextArea = document.createElement('textarea');
                    tempTextArea.value = clipboardData;
                    document.body.appendChild(tempTextArea);
                    tempTextArea.select();
                    document.execCommand('copy');
                    document.body.removeChild(tempTextArea);

                    const msg = `Copied ${rowData.length} rows to clipboard!`;
                    document.getElementById('batchbodyCopy').innerHTML = msg;
                    $('#BatchHistoyCopy').modal('show');

                    //Automatically close popup after 10 seconds
                    setTimeout(() => {
                        $('#BatchHistoyCopy').modal('hide');
                    }, 1000);


                }
            },
            {
                extend: 'csvHtml5',
                className: 'btn-sm button-history btn-outline-success dtbuttons',
                text: '<i class="fa fa-file-csv"></i>',
                titleAttr: 'Export as CSV',
                filename: `BatchHistoryData_${date}`,
                exportOptions: {
                    columns: ':visible'
                },
                action: function (e, dt, button, config) {
                    // Custom CSV generation for large datasets
                    const columnHeaders = dt.columns().header().toArray().map(header => header.innerText);

                    const rowData = dt.rows({ search: 'applied' }).data().toArray().map(row => Object.values(row).join(','));

                    const csvData = [columnHeaders.join(','), ...rowData].join('\n');

                    const blob = new Blob([csvData], { type: 'text/csv;charset=utf-8;' });
                    const link = document.createElement('a');
                    const url = URL.createObjectURL(blob);
                    link.setAttribute('href', url);
                    link.setAttribute('download', `${config.filename}.csv`);
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                }
            },
            {
                extend: 'excelHtml5',
                className: 'btn-sm button-history btn-outline-success dtbuttons',
                text: '<i class="fa fa-file-excel"></i>',
                titleAttr: 'Export as Excel',
                filename: `BatchHistoryData_${date}`,
                exportOptions: {
                    columns: ':visible'  // This will export only visible columns
                },
                action: function (e, dt, button, config) {

                    const workbook = new ExcelJS.Workbook();
                    const worksheet = workbook.addWorksheet("Batch History");

                    const columnHeaders = dt.columns().header().toArray().map(header => header.innerText);
                    const rowData = dt.rows({ search: 'applied' }).data().toArray();

                    worksheet.addRow(["Batch History | Exports Web Portal"]);
                    //worksheet.getCell("A1").font = { bold: true};
                    worksheet.getCell("A1").alignment = { horizontal: "center" };

                    worksheet.mergeCells(1, 1, 1, columnHeaders.length);

                    const headerRow = worksheet.addRow(columnHeaders);
                    headerRow.eachCell(cell => {
                        cell.font = { bold: true };
                        /*cell.alignment = { horizontal: "center", vertical:'center' };*/
                        cell.fill = { type: "pattern" };
                    });

                    rowData.forEach(row => worksheet.addRow(row));

                    worksheet.columns.forEach((col, index) => {
                        let maxLength = 0;
                        col.eachCell({ includeEmpty: true }, cell => {
                            if (cell.value) {
                                const cellLength = cell.value.toString().length;
                                maxLength = Math.max(maxLength, cellLength);
                            }
                        });
                        if (index === worksheet.columns.length - 1) {
                            col.width = maxLength - 13
                        }
                        else if (index === worksheet.columns.length - 2) {
                            col.width = maxLength - 10
                        }
                        else {
                            col.width = maxLength + 1; // Add padding for better readability
                        }
                    });


                    workbook.xlsx.writeBuffer().then(buffer => {
                        const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                        const link = document.createElement("a");
                        link.href = window.URL.createObjectURL(blob);
                        link.download = `BatchHistory_${date}.xlsx`;
                        link.click();
                    });

                }
            },
            {
                extend: 'colvis',
                text: '<i class="fa fa-columns"></i>',
                titleAttr: 'Columns Visibility',
                className: 'btn-sm dropdown-toggle button-history dtbuttons btn-outline-secondary',
                columns: ':not(:first-child)',

            }
        ],
        dom:
            "<'row'<'col-sm-5'B><'col-sm-3'l><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        drawCallback: function () {
            $('.page-link').addClass('btn-sm');
            $('.dataTables_info').addClass('btn-sm');
            $('.dataTables_length').addClass('btn-sm');
            $('.dataTables_filter').addClass('btn-sm');
            var api = this.api();
            if (api.rows().count() > 0) {
                $('.button-history').show();
            }
            else {
                $('.button-history').hide();
            }
            var classButtons = $('.dtbuttons');
            classButtons.each(function () {
                $(this).removeClass('btn-secondary');
            });
        }

    });
    $(document).on('click', '.buttons-csv, .buttons-excel, .buttons-copy', function () {
        const button = $(this);
        button.addClass('processing');
        setTimeout(function () {
            button.removeClass('processing');
        }, 1000);

    });

    var classButtons = document.querySelectorAll('.dtbuttons');

    classButtons.forEach(function (button) {
        button.classList.remove('btn-secondary');
    });
   
}); 














<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Optimized DataTable</title>

    <!-- Include jQuery and DataTables -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/scroller/2.0.7/js/dataTables.scroller.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css"></script>
    <script src="https://cdn.datatables.net/scroller/2.0.7/css/scroller.dataTables.min.css"></script>

    <!-- Include Moment.js -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.30.1/moment.min.js"></script>

    <style>
        /* Optimize table performance */
        table.dataTable thead th {
            white-space: nowrap;
        }
    </style>
</head>
<body>

    <table id="exportProcessDT" class="display nowrap" style="width:100%">
        <thead>
            <tr>
                <th>Batch Key</th>
                <th>Export Process Description</th>
                <th>Export Process Name</th>
                <th>Count</th>
                <th>Static Value</th>
                <th>Export Date</th>
           





$('#selectDates').on('cancel.daterangepicker', function (ev, picker) {
        var last5DaysStart = moment().subtract(4, 'days');
        var last5DaysEnd = moment();

        $('#selectDates').data('daterangepicker').setStartDate(last5DaysStart);
        $('#selectDates').data('daterangepicker').setEndDate(last5DaysEnd);

        var startDate = last5DaysStart.format('MM/DD/YYYY');
        var endDate = last5DaysEnd.format('MM/DD/YYYY');

        $('#dateRangeText').text(startDate + ' - ' + endDate);
        $('#daterange').val(startDate + ' - ' + endDate);
    })



$(document).ready(function () {
    $('#selectDates').daterangepicker({
        opens: "right", // Open to the right
        autoUpdateInput: false, // Prevent auto-filling the input field
        showDropdowns: true, // Allow selecting years and months
        minYear: 1880,
        maxDate: moment(), // Restrict to the current date
        linkedCalendars: false, // Prevents automatic end date change
        autoApply: false, // Requires explicit selection
        isInvalidDate: function(date) {
            // Disable dates that are not in the same month as the selected start date
            var startDate = $('#selectDates').data('daterangepicker').startDate;
            return startDate && date.month() !== startDate.month();
        },
        locale: {
            cancelLabel: 'Clear',
            format: 'MM/DD/YYYY', // Date format
            applyLabel: 'Apply',
            fromLabel: 'From',
            toLabel: 'To',
            separator: ' - '
        },
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    });

    // When a date range is selected
    $('#selectDates').on('apply.daterangepicker', function (ev, picker) {
        var startDate = picker.startDate.format('MM/DD/YYYY');
        var endDate = picker.endDate.format('MM/DD/YYYY');

        $('#dateRangeText').text(startDate + ' - ' + endDate);
        $('#daterange').val(startDate + ' - ' + endDate);
    });

    // Clear selection on cancel
    $('#selectDates').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $('#dateRangeText').text('');
    });

    // Prevent selection outside the same month when choosing a custom date range
    $('#selectDates').on('show.daterangepicker', function(ev, picker) {
        picker.container.find('.available').each(function() {
            var date = moment($(this).attr('data-original-title'), 'MM/DD/YYYY');
            if (picker.startDate && date.month() !== picker.startDate.month()) {
                $(this).addClass('disabled'); // Disable dates from different months
            }
        });
    });
});



$(document).ready(function () {
    // Select DataTables search input field
    $('div.dataTables_filter input[type="search"]').on('input paste', function (event) {
        // Prevent default paste action
        if (event.type === 'paste') {
            event.preventDefault();
            let text = (event.clipboardData || window.clipboardData).getData('text');
            $(this).val(text.replace(/\s+/g, ' ').trim());
        } else {
            // Remove extra spaces dynamically while typing
            $(this).val($(this).val().replace(/\s+/g, ' ').trim());
        }
    });
});


$(document).ready(function () {
    // Select all text and search input fields, including DataTables search
    $('input[type="text"], input[type="search"]').on('input paste', function (event) {
        let inputField = $(this);

        if (event.type === 'paste') {
            event.preventDefault();
            
            // Fix: Ensure clipboardData exists before using getData
            let clipboardData = event.originalEvent.clipboardData || window.clipboardData;
            if (clipboardData) {
                let text = clipboardData.getData('text');
                inputField.val(text.replace(/\s+/g, ' ').trim());
            }
        } else {
            inputField.val(inputField.val().replace(/\s+/g, ' ').trim());
        }
    });
});
$(document).ready(function () {
    // Select all text and search input fields, including DataTables search
    $('input[type="text"], input[type="search"]').on('input paste', function (event) {
        let inputField = $(this);

        if (event.type === 'paste') {
            event.preventDefault();
            let text = (event.clipboardData || window.clipboardData).getData('text');
            inputField.val(text.replace(/\s+/g, ' ').trim());
        } else {
            inputField.val(inputField.val().replace(/\s+/g, ' ').trim());
        }
    });
});




document.querySelectorAll('th input').forEach(input => {
    // Trim spaces on input event
    input.addEventListener('input', function() {
        this.value = this.value.replace(/\s+/g, ' ').trim();
    });

    // Trim spaces on paste event
    input.addEventListener('paste', function(event) {
        event.preventDefault();
        let text = (event.clipboardData || window.clipboardData).getData('text');
        this.value = text.replace(/\s+/g, ' ').trim();
    });
});



$(document).ready(function () {
    function getScaleFactor() {
        var body = document.body;
        var computedStyle = window.getComputedStyle(body);
        var transformMatrix = computedStyle.transform;

        if (transformMatrix !== "none") {
            var values = transformMatrix.split('(')[1].split(')')[0].split(',');
            return parseFloat(values[0]); // Extract scale factor from matrix
        }
        return 1; // Default scale if no transformation is applied
    }

    $('#exportProcessDT').on('contextmenu', 'td', function (e) {
        e.preventDefault();

        var scaleFactor = getScaleFactor(); // Dynamically detect scale

        var table = $('#exportProcessDT').DataTable();
        var rowIndex = table.cell($(this)).index().row;
        var columns = table.columns().header().toArray();

        var idColumnIndex = columns.findIndex(function (element) {
            return $(element).text().trim() === 'Batch Key';
        });

        if (idColumnIndex === -1) {
            console.error("Batch Key column not found");
            return;
        }

        var id = table.cell(rowIndex, idColumnIndex).data();

        if (table.data().count() > 0) {
            $('#exportProcessDT').data('row-id', id);

            $('#contextMenuBatchHistory')
                .css({
                    display: "block",
                    left: (e.pageX / scaleFactor) + "px",
                    top: (e.pageY / scaleFactor) + "px"
                })
                .fadeIn(200);
        }
    });

    // Hide the context menu when clicking elsewhere
    $(document).click(function () {
        $('#contextMenuBatchHistory').fadeOut(200);
    });
});




#contextMenuBatchAuthHistory {
    display: none;
    position: absolute;
    background: white;
    border: 1px solid #ccc;
    box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2);
    z-index: 1000;
    padding: 5px;
    border-radius: 5px;
}

/* Arrow Styles */
#contextMenuBatchAuthHistory::after {
    content: "";
    position: absolute;
    width: 0;
    height: 0;
    border-style: solid;
}

/* Arrow pointing up */
.arrow-top::after {
    left: 50%;
    top: -10px;
    border-width: 0 10px 10px 10px;
    border-color: transparent transparent white transparent;
    transform: translateX(-50%);
}

/* Arrow pointing down */
.arrow-bottom::after {
    left: 50%;
    bottom: -10px;
    border-width: 10px 10px 0 10px;
    border-color: white transparent transparent transparent;
    transform: translateX(-50%);
}

/* Arrow pointing left */
.arrow-left::after {
    left: -10px;
    top: 50%;
    border-width: 10px 10px 10px 0;
    border-color: transparent white transparent transparent;
    transform: translateY(-50%);
}

/* Arrow pointing right */
.arrow-right::after {
    right: -10px;
    top: 50%;
    border-width: 10px 0 10px 10px;
    border-color: transparent transparent transparent white;
    transform: translateY(-50%);
}




$('#authDetailsBatchHistoryDT').on('contextmenu', 'td', function (e) {
    e.preventDefault(); // Prevent default right-click behavior

    var table = $('#authDetailsBatchHistoryDT').DataTable();
    var rowIndex = table.cell($(this)).index().row;
    var columns = table.columns().header().toArray();

    // Find index of "Internal Authorization Id" column
    var idColumnIndex = columns.findIndex(element => 
        $(element).text().trim() === 'Internal Authorization Id'
    );

    if (idColumnIndex === -1) {
        console.error("Column 'Internal Authorization Id' not found");
        return;
    }

    var id = table.cell(rowIndex, idColumnIndex).data();

    if (table.data().count() > 0) {
        $('#authDetailsBatchHistoryDT').data('row-id', id);

        var menu = $('#contextMenuBatchAuthHistory');
        var menuWidth = menu.outerWidth();
        var menuHeight = menu.outerHeight();
        var arrowSize = 10; // Arrow size in pixels

        // Default position
        var posX = e.pageX;
        var posY = e.pageY;

        // Adjust position to prevent overflow
        if (posX + menuWidth > window.innerWidth) {
            posX = window.innerWidth - menuWidth - arrowSize;
            menu.removeClass('arrow-left').addClass('arrow-right'); // Adjust arrow
        } else {
            menu.removeClass('arrow-right').addClass('arrow-left');
        }

        if (posY + menuHeight > window.innerHeight) {
            posY = window.innerHeight - menuHeight - arrowSize;
            menu.removeClass('arrow-top').addClass('arrow-bottom');
        } else {
            menu.removeClass('arrow-bottom').addClass('arrow-top');
        }

        // Set position and show menu
        menu.css({
            display: "block",
            left: posX + "px",
            top: posY + "px"
        });
    }
});

// Hide menu when clicking anywhere
$(document).on('click', function () {
    $('#contextMenuBatchAuthHistory').hide();
});




using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace YourNamespace
{
    internal sealed class YourService : StatelessService
    {
        // Lazy initialization for shared resources
        private static readonly Lazy<X509Certificate2> _certificate = new Lazy<X509Certificate2>(GetCertificateFromStore);
        private static readonly Lazy<ILogger> _logger = new Lazy<ILogger>(() =>
        {
            var seqUri = Environment.GetEnvironmentVariable("Sequri");
            var logFileDir = Environment.GetEnvironmentVariable("LogFileDir") ?? Path.GetTempPath();

            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(logFileDir, "log-.txt"), rollingInterval: RollingInterval.Day)
                .WriteTo.Seq(seqUri)
                .CreateLogger();
        });

        public YourService(StatelessServiceContext context)
            : base(context)
        {
            Log.Logger = _logger.Value; // Ensure logger is initialized
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            // Log instance creation for debugging
            ServiceEventSource.Current.Message($"CreateServiceInstanceListeners invoked. InstanceId: {this.Context.InstanceId}, PartitionId: {this.Context.PartitionId}");

            return new[]
            {
                new ServiceInstanceListener(context =>
                    new KestrelCommunicationListener(context, "ServiceEndpoint", (url, listener) =>
                    {
                        var builder = WebApplication.CreateBuilder();

                        // Add Serilog
                        builder.Host.UseSerilog(Log.Logger);

                        // Add services
                        builder.Services.AddSingleton(context);
                        builder.Services.AddControllers();
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                            {
                                Title = "Your API Service",
                                Version = "v1"
                            });

                            // Include XML documentation if available
                            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                            if (File.Exists(xmlPath))
                            {
                                c.IncludeXmlComments(xmlPath);
                            }
                        });

                        // Configure Kestrel
                        builder.WebHost
                            .UseKestrel(options =>
                            {
                                var endpoint = context.CodePackageActivationContext.GetEndpoint("ServiceEndpoint");
                                options.Listen(IPAddress.IPv6Any, endpoint.Port, listenOptions =>
                                {
                                    listenOptions.UseHttps(_certificate.Value);
                                });
                            })
                            .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.UseUniqueServiceUrl)
                            .UseUrls(url)
                            .UseContentRoot(Directory.GetCurrentDirectory());

                        var app = builder.Build();

                        // Configure the app pipeline
                        if (app.Environment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                            app.UseSwagger();
                            app.UseSwaggerUI(c =>
                            {
                                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Service v1");
                            });
                        }

                        app.UseHttpsRedirection();
                        app.UseRouting();
                        app.UseAuthorization();
                        app.MapControllers();

                        return app;
                    }))
            };
        }

        private static X509Certificate2 GetCertificateFromStore()
        {
            var thumbprint = Environment.GetEnvironmentVariable("ThumbprintId");
            if (string.IsNullOrEmpty(thumbprint))
            {
                throw new InvalidOperationException("ThumbprintId environment variable is not set.");
            }

            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certificate = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, validOnly: false).OfType<X509Certificate2>().FirstOrDefault();
                store.Close();

                if (certificate == null)
                {
                    throw new InvalidOperationException($"Certificate with thumbprint {thumbprint} not found.");
                }

                return certificate;
            }
        }
    }
}








<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Auth ID Validation</title>
  <style>
    .is-valid {
      border: 2px solid green;
    }

    .is-invalid {
      border: 2px solid red;
    }
  </style>
</head>
<body>
  <div>
    <label for="authId">Auth ID:</label>
    <input type="text" id="authId" placeholder="Enter Auth ID">
  </div>
  <script>
    // Debounce function to reduce event frequency
    function debounce(func, delay) {
      let timeout;
      return function (...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), delay);
      };
    }

    // Add event listener to input field
    document.getElementById("authId").addEventListener(
      'input',
      debounce(function () {
        const authId = document.getElementById('authId');
        const value = authId.value.trim();

        // Validate the input
        if (value !== "") {
          authId.classList.remove("is-invalid");
          authId.classList.add("is-valid");
        } else {
          authId.classList.remove("is-valid");
          authId.classList.add("is-invalid");
        }
      }, 200) // Debounce delay of 200ms
    );
  </script>
</body>
</html>




<div class="form-group input-group input-group-sm row">
    <label for="exportProcessName" class="mr-2 mt-1">Export Process:</label>
    <div class="dropdown">
        <button class="btn btn-secondary dropdown-toggle form-control form-control-sm col-6 col-md-4 col-sm-4 ml-1" type="button" id="exportProcessDropdown" data-bs-toggle="dropdown" aria-expanded="false">
            Select Export Process
        </button>
        <ul class="dropdown-menu" aria-labelledby="exportProcessDropdown">
            <li>
                <a class="dropdown-item ttip-opt" href="#" title="All">All</a>
            </li>
            @foreach (dynamic item in ViewBag.exportProcesses)
            {
                <li>
                    <a class="dropdown-item ttip-opt" href="#" title="@item.Name">@item.ExportProcessDescription</a>
                </li>
            }
        </ul>
    </div>
</div>

<script>
$(document).ready(function () {
    // Initialize tooltips for dropdown items
    $('.ttip-opt').tooltip({
        html: true, // Enables HTML inside the tooltip
        placement: 'right' // Adjust placement as needed
    });
});
</script>

<!-- Include Bootstrap and jQuery -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
<script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min



<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
<script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>



<label for="exportProcessName" class="mr-2 mt-1">Export Process:</label>
    <select class="form-control form-control-sm col-6 col-md-4 col-sm-4" 
            name="exportProcessName" 
            id="exportProcessName" 
            data-toggle="tooltip" 
            data-html="true" 
            title="Select an Export Process">
        <option value="All" title="Select All Processes">All</option>
        @foreach (dynamic item in ViewBag.exportProcesses)
        {
            <option value="@item.Name" title="@item.Name" @(item.Name.Equals(ViewBag.exportProcessNameParam) ? "selected" : "")>
                @item.Name
            </option>
        }
    </select>


$(document).ready(function () {
    // Initialize tooltip
    $('[data-toggle="tooltip"]').tooltip();

    // Update tooltip dynamically when selection changes
    $('#exportProcessName').on('change', function () {
        var selectedText = $(this).find('option:selected').attr('title'); // Get the title of the selected option
        $(this).attr('title', selectedText).tooltip('dispose').tooltip(); // Update and reinitialize the tooltip
    });

    // Set initial tooltip value
    var initialText = $('#exportProcessName').find('option:selected').attr('title');
    $('#exportProcessName').attr('title', initialText).tooltip('dispose').tooltip();
});



#selectDates {
    display: flex;
    align-items: center;
    justify-content: space-between;
    border: 1px solid #ced4da;
    padding: 5px;
    width: 100%; /* Adjust width if necessary */
    min-width: 150px; /* Prevent content overflow */
}

#dateRangeText {
    flex: 1;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    padding-left: 5px;
}


<div id="selectDates" class="selectbox form-control pull-right form-control-sm col-4 col-md-3 col-sm-3 mr-3">
    <i class="fa fa-calendar-alt mr-2"></i>
    <input
        type="text"
        id="dateRangeText"
        class="form-control-sm"
        value="@ViewBag.daterangeparam"
        placeholder="Select Date"
        style="border: none; flex: 1; outline: none;">
    <i class="fas fa-caret-down caret"></i>
</div>
worksheet.columns.forEach((col, index) => {
        let maxLength = 0;
        col.eachCell({ includeEmpty: true }, cell => {
            if (cell.value) {
                const cellLength = cell.value.toString().length;
                maxLength = Math.max(maxLength, cellLength);
            }
        });
        col.width = maxLength + 2; // Add padding for better readability
    });



worksheet.columns = columnHeaders.map((header, colIndex) => {
        const maxContentLength = Math.max(
            header.length, // Header length
            ...rowData.map(row => row[colIndex]?.toString().length || 0) // Length of each cell in the column
        );

        return {
            header: header,
            key: `col${colIndex + 1}`,
            width: maxContentLength + 2, // Add padding to width
        };
    });

function exportToExcel(dt) {
    const workbook = new ExcelJS.Workbook();
    const worksheet = workbook.addWorksheet("Batch History");

    const columnHeaders = dt.columns().header().toArray().map(header => header.innerText);
    const rowData = dt.rows({ search: 'applied' }).data().toArray();

    worksheet.addRow(["Batch History | Exports Web Portal"]);
    worksheet.getCell("A1").font = { bold: true, size: 16, color: { argb: "FF000000" } };
    worksheet.getCell("A1").alignment = { horizontal: "center" };

    worksheet.mergeCells(1, 1, 1, columnHeaders.length);

    worksheet.addRow([]);
    worksheet.addRow(columnHeaders).eachCell(cell => {
        cell.font = { bold: true };
        cell.alignment = { horizontal: "center" };
        cell.fill = { type: "pattern", pattern: "solid", fgColor: { argb: "FFDDDDDD" } };
    });

    rowData.forEach(row => worksheet.addRow(row));

    workbook.xlsx.writeBuffer().then(buffer => {
        const blob = new Blob([buffer], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
        const link = document.createElement("a");
        link.href = window.URL.createObjectURL(blob);
        link.download = "BatchHistory_Export.xlsx";
        link.click();
    });
}




<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Date Range Picker</title>
  <!-- Include CSS for Bootstrap and Daterangepicker -->
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.2/dist/css/bootstrap.min.css">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css">
  <!-- FontAwesome for Icons -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
  <style>
    .selectbox {
      cursor: pointer;
      display: inline-block;
      padding: 10px;
      background: #fff;
      border: 1px solid #ccc;
      border-radius: 4px;
    }

    .selectbox .fa-calendar {
      margin-right: 8px;
    }

    .selectbox .caret {
      margin-left: 5px;
    }
  </style>
</head>
<body>
  <div class="container mt-5">
    <div id="daterange" class="selectbox pull-right">
      <i class="fa fa-calendar"></i>
      <span>Select Date Range</span>
      <b class="caret"></b>
    </div>
  </div>

  <!-- Include JS for jQuery, Moment.js, and Daterangepicker -->
  <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
  <script src="https://cdn.jsdelivr.net/momentjs/latest/moment.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>
  <script>
    $(document).ready(function () {
      // Initialize the date range picker
      $('#daterange').daterangepicker(
        {
          opens: 'right', // Open to the right
          autoUpdateInput: false, // Prevent auto update of input field
          locale: {
            cancelLabel: 'Clear',
            format: 'MM/DD/YYYY', // Date format
          },
          ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
          },
        },
        function (start, end) {
          // Callback for when a date is selected
          $('#daterange span').html(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
        }
      );

      // Clear button functionality
      $('#daterange').on('cancel.daterangepicker', function () {
        $(this).find('span').html('Select Date Range');
      });
    });
  </script>
</body>
</html>





{
    extend: 'copyHtml5',
    className: 'button-BatchHistory btn-outline-primary dtbuttons',
    text: '<i class="fa fa-copy"></i>',
    titleAttr: 'Copy',
    exportOptions: {
        columns: ':visible' // Export only visible columns
    },
    action: function (e, dt, button, config) {
        // Extract headers
        const columnHeaders = dt.columns({ search: 'applied' }).header().toArray().map(header => header.innerText);

        // Extract rows data
        const rowData = dt.rows({ search: 'applied' }).data().toArray().map(row => 
            Object.values(row).join('\t') // Convert each row object into tab-separated values
        );

        // Combine headers and rows into the final clipboard text
        const clipboardData = [columnHeaders.join('\t'), ...rowData].join('\n');

        // Use Clipboard API to copy data
        navigator.clipboard.writeText(clipboardData)
            .then(() => {
                // Show popup notification
                const popup = document.createElement('div');
                popup.innerText = `Copied ${rowData.length} rows to clipboard!`;
                popup.style.position = 'fixed';
                popup.style.bottom = '20px';
                popup.style.right = '20px';
                popup.style.padding = '10px 20px';
                popup.style.backgroundColor = '#4CAF50'; // Green background
                popup.style.color = '#fff'; // White text
                popup.style.borderRadius = '5px';
                popup.style.boxShadow = '0 4px 8px rgba(0, 0, 0, 0.2)';
                popup.style.zIndex = '9999';

                document.body.appendChild(popup);

                // Automatically close popup after 3 seconds
                setTimeout(() => {
                    popup.remove();
                }, 3000);
            })
            .catch(err => {
                console.error('Failed to copy data to clipboard', err);
            });
    }
}


{
    extend: 'excelHtml5',
    className: 'button-BatchHistory btn-outline-success dtbuttons',
    text: '<i class="fa fa-file-excel"></i>',
    titleAttr: 'Export as Excel',
    filename: `BatchHistoryData_${new Date().toISOString().split('T')[0]}`,
    exportOptions: {
        columns: ':visible'  // This will export only visible columns
    },
    action: function (e, dt, button, config) {
        const wb = XLSX.utils.book_new(); // Create a new workbook
        
        // Get the column headers dynamically from the table
        const columnHeaders = dt.columns().header().toArray().map(header => header.innerText);
        
        // Get the data (rows) for the visible columns
        const rowData = dt.rows({ search: 'applied' }).data().toArray().map(row => {
            return columnHeaders.map(col => row[col.toLowerCase().replace(/\s/g, '')]); // Map row data based on column headers
        });

        // Combine headers and rows into the final data array
        const data = [columnHeaders, ...rowData];  // Adding headers as the first row
        
        // Convert the data to a sheet
        const ws = XLSX.utils.aoa_to_sheet(data);

        // Apply bold styling to the header row
        const headerStyle = { font: { bold: true } };
        Object.keys(ws).forEach(cell => {
            if (cell.startsWith('A1') || cell.startsWith('B1') || cell.startsWith('C1') || cell.startsWith('D1')) {
                ws[cell].s = headerStyle;
            }
        });

        // Append the sheet to the workbook
        XLSX.utils.book_append_sheet(wb, ws, 'BatchHistory');
        
        // Write the file
        XLSX.writeFile(wb, `${config.filename}.xlsx`);
    }
}


<!DOCTYPE html>
<html>
<head>
    <title>DataTable Example</title>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css">
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.dataTables.min.css">
</head>
<body>
    <table id="example" class="display" style="width:100%">
        <thead>
            <tr>
                <th>Export Process</th>
                <th>Batch Key</th>
                <th>Export Date</th>
                <th>Count</th>
            </tr>
        </thead>
        <tbody>
            <!-- Dynamic Data will be loaded here by JavaScript -->
        </tbody>
    </table>

    <!-- Include JS Libraries -->
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.html5.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.print.min.js"></script>

    <script>
        $(document).ready(function () {
            const largeDataset = [];
            for (let i = 0; i < 50000; i++) {
                largeDataset.push({
                    exportProcessName: `Export_Process_${i + 1}`,
                    batchKey: `Key_${i + 1}`,
                    exportDate: new Date().toISOString(),
                    count: Math.floor(Math.random() * 1000)
                });
            }

            const dt = $('#example').DataTable({
                data: largeDataset,
                columns: [
                    { data: 'exportProcessName' },
                    { data: 'batchKey' },
                    { data: 'exportDate' },
                    { data: 'count' }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fa fa-file-excel"></i> Excel',
                        className: 'btn-sm btn-outline-success',
                        titleAttr: 'Export to Excel',
                        filename: `BatchHistoryData_${new Date().toISOString().split('T')[0]}`,
                        action: function (e, dt, button, config) {
                            const wb = XLSX.utils.book_new();
                            const columnHeaders = dt.columns().header().toArray().map(header => header.innerText); // Get column headers dynamically
                            const data = [
                                columnHeaders, // Add dynamic column headers
                                ...dt.rows({ search: 'applied' }).data().toArray().map(row => [
                                    row.exportProcessName,
                                    row.batchKey,
                                    row.exportDate,
                                    row.count
                                ])
                            ];
                            const ws = XLSX.utils.aoa_to_sheet(data);
                            const headerStyle = { font: { bold: true } };
                            Object.keys(ws).forEach(cell => {
                                if (cell.startsWith('A1') || cell.startsWith('B1') || cell.startsWith('C1') || cell.startsWith('D1')) {
                                    ws[cell].s = headerStyle;
                                }
                            });
                            XLSX.utils.book_append_sheet(wb, ws, 'BatchHistory');
                            XLSX.writeFile(wb, `${config.filename}.xlsx`);
                        }
                    },
                    {
                        extend: 'csvHtml5',
                        text: '<i class="fa fa-file-csv"></i> CSV',
                        className: 'btn-sm btn-outline-primary',
                        titleAttr: 'Export to CSV',
                        filename: `BatchHistoryData_${new Date().toISOString().split('T')[0]}`,
                        customize: function (csv) {
                            const columnHeaders = dt.columns().header().toArray().map(header => header.innerText); // Get column headers dynamically
                            const csvWithHeader = columnHeaders.join(',') + '\n' + csv;
                            return csvWithHeader;
                        }
                    },
                    {
                        extend: 'copyHtml5',
                        text: '<i class="fa fa-copy"></i> Copy',
                        className: 'btn-sm btn-outline-secondary',
                        titleAttr: 'Copy Data',
                        exportOptions: {
                            columns: ':visible'
                        }
                    }
                ]
            });
        });
    </script>
</body>
</html>



<!DOCTYPE html>
<html>
<head>
    <title>DataTable Example</title>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css">
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.dataTables.min.css">
</head>
<body>
    <table id="example" class="display" style="width:100%"></table>

    <!-- Include JS Libraries -->
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.html5.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.print.min.js"></script>

    <script>
        $(document).ready(function () {
            const largeDataset = [];
            for (let i = 0; i < 50000; i++) {
                largeDataset.push({
                    exportProcessName: `Export_Process_${i + 1}`,
                    batchKey: `Key_${i + 1}`,
                    exportDate: new Date().toISOString(),
                    count: Math.floor(Math.random() * 1000)
                });
            }

            const columns = [
                { data: 'exportProcessName', title: 'Export Process' },
                { data: 'batchKey', title: 'Batch Key' },
                { data: 'exportDate', title: 'Export Date' },
                { data: 'count', title: 'Count' }
            ];

            $('#example').DataTable({
                data: largeDataset,
                columns: columns,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fa fa-file-excel"></i> Excel',
                        className: 'btn-sm btn-outline-success',
                        titleAttr: 'Export to Excel',
                        filename: `BatchHistoryData_${new Date().toISOString().split('T')[0]}`,
                        action: function (e, dt, button, config) {
                            const wb = XLSX.utils.book_new();
                            const data = [
                                columns.map(col => col.title), // Header row from column titles
                                ...dt.rows({ search: 'applied' }).data().toArray().map(row => 
                                    columns.map(col => row[col.data] || '') // Row data dynamically based on column titles
                                )
                            ];
                            const ws = XLSX.utils.aoa_to_sheet(data);
                            const headerStyle = { font: { bold: true } };
                            Object.keys(ws).forEach(cell => {
                                if (cell.startsWith('A1') || cell.startsWith('B1') || cell.startsWith('C1') || cell.startsWith('D1')) {
                                    ws[cell].s = headerStyle;
                                }
                            });
                            XLSX.utils.book_append_sheet(wb, ws, 'BatchHistory');
                            XLSX.writeFile(wb, `${config.filename}.xlsx`);
                        }
                    },
                    {
                        extend: 'csvHtml5',
                        text: '<i class="fa fa-file-csv"></i> CSV',
                        className: 'btn-sm btn-outline-primary',
                        titleAttr: 'Export to CSV',
                        filename: `BatchHistoryData_${new Date().toISOString().split('T')[0]}`,
                        customize: function (csv) {
                            const headerRow = columns.map(col => col.title).join(',') + '\n';
                            return headerRow + csv;
                        }
                    },
                    {
                        extend: 'copyHtml5',
                        text: '<i class="fa fa-copy"></i> Copy',
                        className: 'btn-sm btn-outline-secondary',
                        titleAttr: 'Copy Data',
                        exportOptions: {
                            columns: ':visible'
                        }
                    }
                ]
            });
        });
    </script>
</body>
</html>





$(document).ready(function () {
    // Simulate large dataset
    const largeDataset = [];
    for (let i = 0; i < 130000; i++) {
        largeDataset.push({
            exportProcessName: `Export_Process_${i + 1}`,
            batchKey: `Key_${i + 1}`,
            exportDate: new Date().toISOString(),
            count: Math.floor(Math.random() * 1000)
        });
    }

    // Initialize DataTable
    $('#example').DataTable({
        data: largeDataset,
        processing: true,    // Show a processing indicator
        deferRender: true,   // Defer rendering for better performance
        scrollY: '60vh',     // Virtual scrolling height
        scrollCollapse: true, // Collapse unused space
        scroller: true,      // Enable scroller
        pageLength: 50,      // Display 50 rows per page
        dom: 'Bfrtip',       // Enable export buttons
        buttons: [
            {
                extend: 'copyHtml5',
                className: 'btn-sm btn-outline-primary',
                text: '<i class="fa fa-copy"></i> Copy',
                titleAttr: 'Copy',
                exportOptions: {
                    columns: ':visible'
                },
                action: function (e, dt, button, config) {
                    // Custom implementation for faster copying
                    navigator.clipboard.writeText(
                        dt.rows({ search: 'applied' }).data().toArray().map(row =>
                            Object.values(row).join('\t')
                        ).join('\n')
                    );
                    alert('Copied to clipboard!');
                }
            },
            {
                extend: 'csvHtml5',
                className: 'btn-sm btn-outline-success',
                text: '<i class="fa fa-file-csv"></i> CSV',
                titleAttr: 'Export as CSV',
                filename: `BatchHistoryData_${new Date().toISOString().split('T')[0]}`,
                exportOptions: {
                    columns: ':visible'
                },
                action: function (e, dt, button, config) {
                    // Custom CSV generation for large datasets
                    const csvData = dt.rows({ search: 'applied' }).data().toArray()
                        .map(row => Object.values(row).join(',')).join('\n');
                    const blob = new Blob([csvData], { type: 'text/csv;charset=utf-8;' });
                    const link = document.createElement('a');
                    const url = URL.createObjectURL(blob);
                    link.setAttribute('href', url);
                    link.setAttribute('download', `${config.filename}.csv`);
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                }
            },
            {
                extend: 'excelHtml5',
                className: 'btn-sm btn-outline-success',
                text: '<i class="fa fa-file-excel"></i> Excel',
                titleAttr: 'Export as Excel',
                filename: `BatchHistoryData_${new Date().toISOString().split('T')[0]}`,
                exportOptions: {
                    columns: ':visible'
                },
                action: function (e, dt, button, config) {
                    // Custom Excel generation using XLSX.js
                    const wb = XLSX.utils.book_new();
                    const ws = XLSX.utils.json_to_sheet(dt.rows({ search: 'applied' }).data().toArray());
                    XLSX.utils.book_append_sheet(wb, ws, 'BatchHistory');
                    XLSX.writeFile(wb, `${config.filename}.xlsx`);
                }
            }
        ],
        columns: [
            { data: 'exportProcessName', title: 'Export Process' },
            { data: 'batchKey', title: 'Batch Key' },
            { data: 'exportDate', title: 'Export Date' },
            { data: 'count', title: 'Count' }
        ]
    });
});







<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Datepicker with Month and Year Select</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <style>
        .error-day a { background-color: red !important; color: white !important; }
        .success-day a { background-color: green !important; color: white !important; }
        .no-record-day a { background-color: gray !important; color: white !important; }
        #custom-controls {
            margin-bottom: 10px;
        }
        #custom-controls select {
            margin-right: 10px;
        }
    </style>
</head>
<body>

    <h3>Select a Date to View Auth Response</h3>

    <div id="custom-controls">
        <!-- Dropdowns for month and year -->
        <label for="month-select">Month:</label>
        <select id="month-select"></select>

        <label for="year-select">Year:</label>
        <select id="year-select"></select>
    </div>

    <!-- Input for the datepicker -->
    <input type="text" id="datepicker" />

    <script>
        $(document).ready(function() {
            const authResponses = [
                { date: '2024-12-01', response: 'Error' },
                { date: '2024-12-02', response: 'Success' },
                { date: '2024-12-05', response: 'No Record' }
            ];

            const today = new Date();

            // Populate month dropdown
            const monthSelect = $('#month-select');
            const months = [
                'January', 'February', 'March', 'April', 'May', 'June', 
                'July', 'August', 'September', 'October', 'November', 'December'
            ];
            months.forEach((month, index) => {
                monthSelect.append(`<option value="${index}" ${index === today.getMonth() ? 'selected' : ''}>${month}</option>`);
            });

            // Populate year dropdown
            const yearSelect = $('#year-select');
            const startYear = today.getFullYear() - 10; // Start 10 years before current year
            const endYear = today.getFullYear() + 10;  // End 10 years after current year
            for (let year = startYear; year <= endYear; year++) {
                yearSelect.append(`<option value="${year}" ${year === today.getFullYear() ? 'selected' : ''}>${year}</option>`);
            }

            // Initialize Datepicker
            $('#datepicker').datepicker({
                beforeShowDay: function(date) {
                    const dateString = $.datepicker.formatDate('yy-mm-dd', date);
                    let className = '';

                    // Loop through authResponses and check if the date matches
                    authResponses.forEach(item => {
                        if (item.date === dateString) {
                            switch (item.response) {
                                case 'Error':
                                    className = 'error-day';
                                    break;
                                case 'Success':
                                    className = 'success-day';
                                    break;
                                case 'No Record':
                                    className = 'no-record-day';
                                    break;
                            }
                        }
                    });

                    return [true, className];  // Return true to enable the date, and add class for color
                },
                onChangeMonthYear: function(year, month) {
                    $('#month-select').val(month - 1); // Update dropdowns when datepicker changes
                    $('#year-select').val(year);
                }
            });

            // Sync Datepicker with dropdowns
            const syncDatepicker = () => {
                const year = $('#year-select').val();
                const month = $('#month-select').val();
                const newDate = new Date(year, month, 1); // Set date to the 1st of the selected month and year
                $('#datepicker').datepicker('setDate', newDate);
                $('#datepicker').datepicker('refresh');
            };

            // Change Datepicker when month or year dropdown changes
            $('#month-select, #year-select').on('change', syncDatepicker);

            // Styling for custom classes (Error, Success, No Record)
            $('<style>')
                .prop('type', 'text/css')
                .html(`
                    .error-day a { background-color: red !important; color: white !important; }
                    .success-day a { background-color: green !important; color: white !important; }
                    .no-record-day a { background-color: gray !important; color: white !important; }
                `)
                .appendTo('head');

            // Sync initial Datepicker state
            syncDatepicker();
        });
    </script>

</body>
</html>





<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Customize Datepicker Month/Year</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <style>
        .error-day a { background-color: red !important; color: white !important; }
        .success-day a { background-color: green !important; color: white !important; }
        .no-record-day a { background-color: gray !important; color: white !important; }
    </style>
</head>
<body>

    <h3>Select a Date to View Auth Response</h3>

    <!-- Input for the date picker -->
    <input type="text" id="datepicker" />

    <div>
        <!-- Buttons to change the month/year -->
        <button id="prevMonth">Previous Month</button>
        <button id="nextMonth">Next Month</button>
        <button id="setDate">Set Specific Date</button>
    </div>

    <script>
        $(document).ready(function() {
            const authResponses = [
                { date: '2024-12-01', response: 'Error' },
                { date: '2024-12-02', response: 'Success' },
                { date: '2024-12-05', response: 'No Record' }
            ];

            // Initialize Datepicker
            $('#datepicker').datepicker({
                beforeShowDay: function(date) {
                    const dateString = $.datepicker.formatDate('yy-mm-dd', date);
                    let className = '';

                    // Loop through authResponses and check if the date matches
                    authResponses.forEach(item => {
                        if (item.date === dateString) {
                            switch (item.response) {
                                case 'Error':
                                    className = 'error-day';
                                    break;
                                case 'Success':
                                    className = 'success-day';
                                    break;
                                case 'No Record':
                                    className = 'no-record-day';
                                    break;
                            }
                        }
                    });

                    return [true, className];  // Return true to enable the date, and add class for color
                }
            });

            // Styling for custom classes (Error, Success, No Record)
            $('<style>')
                .prop('type', 'text/css')
                .html(`
                    .error-day a { background-color: red !important; color: white !important; }
                    .success-day a { background-color: green !important; color: white !important; }
                    .no-record-day a { background-color: gray !important; color: white !important; }
                `)
                .appendTo('head');

            // Change to previous month when "Previous Month" is clicked
            $('#prevMonth').click(function() {
                var currentDate = $('#datepicker').datepicker('getDate');
                currentDate.setMonth(currentDate.getMonth() - 1);
                $('#datepicker').datepicker('setDate', currentDate); // Set new date
            });

            // Change to next month when "Next Month" is clicked
            $('#nextMonth').click(function() {
                var currentDate = $('#datepicker').datepicker('getDate');
                currentDate.setMonth(currentDate.getMonth() + 1);
                $('#datepicker').datepicker('setDate', currentDate); // Set new date
            });

            // Set a specific date when "Set Specific Date" is clicked
            $('#setDate').click(function() {
                var newDate = new Date(2024, 11, 5); // Set to December 5, 2024 (Note: months are 0-based)
                $('#datepicker').datepicker('setDate', newDate); // Set new date
            });
        });
    </script>

</body>
</html>



<div class="container mt-5">
        <div class="row">
            <div class="col-md-6">
                <!-- Label and Date Input Field Side by Side -->
                <div class="form-group row">
                    <label for="date-picker" class="col-md-4 col-form-label mt-1">Select Date to View Auth Response</label>
                    <div class="col-md-8">
                        <!-- Input Group with Calendar Icon Inside -->
                        <div class="input-group">
                            <input type="text" class="form-control input-sm" id="date-picker" placeholder="Select a Date">
                            <div class="input-group-append">
                                <span class="input-group-text">
                                    <i class="fas fa-calendar"></i>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Custom Calendar with Date Color Marking</title>

    <!-- jQuery and jQuery UI CSS/JS -->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

    <style>
        /* Custom styles for color-coding */
        .error-day a {
            background-color: red !important;
            color: white !important;
        }
        .success-day a {
            background-color: green !important;
            color: white !important;
        }
        .no-record-day a {
            background-color: gray !important;
            color: white !important;
        }
    </style>
</head>
<body>

    <h3>Select Date to View Response</h3>
    <input type="text" id="datepicker" />

    <div id="responseValue" hidden>
        <!-- This section will hold the raw data to simulate passing from the server -->
        <script type="application/json" id="responseData">
            [
                { "date": "2024-12-01", "response": "Error" },
                { "date": "2024-12-02", "response": "Success" },
                { "date": "2024-12-05", "response": "No Record" }
            ]
        </script>
    </div>

    <script>
        $(document).ready(function() {
            // Parse the JSON data from the hidden div
            const authResponses = JSON.parse($('#responseData').text());

            // Initialize the datepicker
            $('#datepicker').datepicker({
                beforeShowDay: function(date) {
                    const dateString = $.datepicker.formatDate('yy-mm-dd', date);
                    let className = '';

                    // Loop through authResponses and check if the date matches
                    authResponses.forEach(item => {
                        if (item.date === dateString) {
                            switch (item.response) {
                                case 'Error':
                                    className = 'error-day';
                                    break;
                                case 'Success':
                                    className = 'success-day';
                                    break;
                                case 'No Record':
                                    className = 'no-record-day';
                                    break;
                            }
                        }
                    });

                    // Return true to enable the date and set the class for styling
                    return [true, className];
                }
            });

            // Styling for custom classes (added dynamically in the header)
            $('<style>')
                .prop('type', 'text/css')
                .html(`
                    .error-day a { background-color: red !important; color: white !important; }
                    .success-day a { background-color: green !important; color: white !important; }
                    .no-record-day a { background-color: gray !important; color: white !important; }
                `)
                .appendTo('head');
        });
    </script>

</body>
</html>




document.addEventListener('DOMContentLoaded', () => {
    // Access the `authResponses` variable passed from the Razor view
    const responses = window.authResponses || [];

    // Get the date picker element
    const datePicker = document.getElementById('date-picker');

    // Set colors on page load
    responses.forEach(response => {
        const date = new Date(response.CreatedDate);
        const responseType = response.ResponseType;

        if (responseType.includes("Error")) {
            setDateColor(date, 'red');
        } else if (responseType.includes("Success")) {
            setDateColor(date, 'green');
        } else {
            setDateColor(date, 'gray'); // Default color for normal
        }
    });

    // Function to set background color for specific dates
    function setDateColor(date, color) {
        // Format the date as 'YYYY-MM-DD'
        const formattedDate = date.toISOString().split('T')[0];

        // Check if the date matches the picker value (if the date picker shows multiple dates, customize as needed)
        const calendarDates = document.querySelectorAll(`input[type="date"]`);
        calendarDates.forEach(input => {
            if (input.value === formattedDate) {
                input.style.backgroundColor = color;
            }
        });
    }
});




document.addEventListener('DOMContentLoaded', function () {
    const authResponses = @Html.Raw(ViewBag.AuthResponsesJson);

    // Function to generate a simple calendar
    function generateCalendar() {
        const calendar = document.getElementById('calendar');
        const today = new Date();
        const currentMonth = today.getMonth();
        const currentYear = today.getFullYear();

        // Get the first and last day of the current month
        const firstDay = new Date(currentYear, currentMonth, 1);
        const lastDay = new Date(currentYear, currentMonth + 1, 0);

        // Populate calendar days
        for (let day = 1; day <= lastDay.getDate(); day++) {
            const date = new Date(currentYear, currentMonth, day);
            const dateString = date.toISOString().split('T')[0];

            // Create a day div
            const dayDiv = document.createElement('div');
            dayDiv.className = 'calendar-day';
            dayDiv.textContent = day;

            // Check for matching auth responses
            const response = authResponses.find(r => r.Date === dateString);
            if (response) {
                if (response.ResponseType.includes("Error")) {
                    dayDiv.classList.add('bg-danger');
                } else {
                    dayDiv.classList.add('bg-success');
                }
            }

            calendar.appendChild(dayDiv);
        }
    }

    generateCalendar();

    // Handle date selection
    document.getElementById('date-picker').addEventListener('change', function () {
        const selectedDate = this.value;
        const response = authResponses.find(r => r.Date === selectedDate);
        const errorText = document.getElementById('errorText');

        if (!response) {
            errorText.style.display = 'block';
        } else {
            errorText.style.display = 'none';
        }
    });
});


document.addEventListener('DOMContentLoaded', function () {
    // Retrieve the date input element and hidden response items
    const datePicker = document.getElementById('date-picker');
    const authResponseItems = document.querySelectorAll('.authResponseItem');

    // Create a map to store date-response types
    const dateResponseMap = new Map();

    // Populate the map with response data
    authResponseItems.forEach(item => {
        const date = item.getAttribute('data-date');
        const responseType = item.getAttribute('data-type');

        if (date && responseType) {
            dateResponseMap.set(date, responseType);
        }
    });

    // Handle calendar input change
    datePicker.addEventListener('change', function () {
        const selectedDate = datePicker.value;
        const responseType = dateResponseMap.get(selectedDate);

        const errorText = document.getElementById('errorText');

        if (responseType) {
            // Update the background color based on the response type
            if (responseType.includes("Error")) {
                datePicker.style.backgroundColor = 'red';
            } else if (responseType.includes("Success")) {
                datePicker.style.backgroundColor = 'green';
            } else {
                datePicker.style.backgroundColor = 'lightgray';
            }
            errorText.style.display = 'none';
        } else {
            // No record for the selected date
            datePicker.style.backgroundColor = '';
            errorText.style.display = 'block';
        }
    });
});





<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Real-Time Authorization with Date Selection</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/flatpickr@4.6.9/dist/flatpickr.min.css" rel="stylesheet">
    <style>
        .timeline {
            position: relative;
            padding: 20px;
            border-left: 3px solid #007bff;
            margin-top: 20px;
        }
        .timeline-item {
            margin-bottom: 20px;
            padding-left: 20px;
            position: relative;
        }
        .timeline-item::before {
            content: '';
            position: absolute;
            left: -11px;
            top: 0;
            width: 20px;
            height: 20px;
            background-color: #007bff;
            border-radius: 50%;
            z-index: 1;
        }
        .timeline-date {
            font-weight: bold;
            margin-bottom: 5px;
            color: #007bff;
        }
        .timeline-content {
            padding: 10px 15px;
            background: #f8f9fa;
            border-radius: 5px;
            border: 1px solid #e0e0e0;
        }
    </style>
</head>
<body>
    <div class="container mt-5">
        <!-- Calendar Selector -->
        <div class="mb-4">
            <h4>Select Date to View Authorization Errors</h4>
            <input type="text" id="date-picker" class="form-control" placeholder="Select a Date">
        </div>

        <!-- Timeline Section -->
        <div id="timeline-container">
            <!-- Timeline content is embedded in HTML and will be filtered by JS -->
            <div class="timeline-item" data-date="7/19/2023 6:12:05 AM" data-type="Error" data-source="AuthExport_Realtime_CIGNA_GTM" data-authid="EVIRT002" data-code="EVIRT002" data-description="General Exception: Failure">
                <div class="timeline-date">7/19/2023 6:12:05 AM</div>
                <div class="timeline-content">
                    <h5><span class="badge bg-danger">Error</span></h5>
                    <p><strong>Source:</strong> AuthExport_Realtime_CIGNA_GTM</p>
                    <p><strong>AuthID Received:</strong> EVIRT002</p>
                    <p><strong>ErrorCode:</strong> EVIRT002</p>
                    <p><strong>ErrorDescription:</strong> General Exception: Failure</p>
                    <button class="btn btn-primary btn-sm" onclick="viewDetails(this)">View Details</button>
                </div>
            </div>
            <div class="timeline-item" data-date="8/11/2023 12:47:39 PM" data-type="Error" data-source="AuthExport_Realtime_CIGNA_GTM" data-authid="EVIRT002" data-code="EVIRT002" data-description="General Exception: Failure">
                <div class="timeline-date">8/11/2023 12:47:39 PM</div>
                <div class="timeline-content">
                    <h5><span class="badge bg-danger">Error</span></h5>
                    <p><strong>Source:</strong> AuthExport_Realtime_CIGNA_GTM</p>
                    <p><strong>AuthID Received:</strong> EVIRT002</p>
                    <p><strong>ErrorCode:</strong> EVIRT002</p>
                    <p><strong>ErrorDescription:</strong> General Exception: Failure</p>
                    <button class="btn btn-primary btn-sm" onclick="viewDetails(this)">View Details</button>
                </div>
            </div>
            <div class="timeline-item" data-date="8/16/2023 3:46:30 PM" data-type="Error" data-source="AuthExport_Realtime_CIGNA_GTM" data-authid="EVIRT001" data-code="EVIRT001" data-description="An error occurred while processing the request">
                <div class="timeline-date">8/16/2023 3:46:30 PM</div>
                <div class="timeline-content">
                    <h5><span class="badge bg-danger">Error</span></h5>
                    <p><strong>Source:</strong> AuthExport_Realtime_CIGNA_GTM</p>
                    <p><strong>AuthID Received:</strong> EVIRT001</p>
                    <p><strong>ErrorCode:</strong> EVIRT001</p>
                    <p><strong>ErrorDescription:</strong> An error occurred while processing the request</p>
                    <button class="btn btn-primary btn-sm" onclick="viewDetails(this)">View Details</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal (for error details) -->
    <div class="modal fade" id="errorModal" tabindex="-1" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="errorModalLabel">Error Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p><strong>Date:</strong> <span id="error-date"></span></p>
                    <p><strong>Type:</strong> <span id="error-type"></span></p>
                    <p><strong>Source:</strong> <span id="error-source"></span></p>
                    <p><strong>AuthID Received:</strong> <span id="error-authid"></span></p>
                    <p><strong>ErrorCode:</strong> <span id="error-code"></span></p>
                    <p><strong>ErrorDescription:</strong> <span id="error-description"></span></p>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/flatpickr@4.6.9/dist/flatpickr.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // Initialize the date picker
        flatpickr("#date-picker", {
            dateFormat: "m/d/Y",
            onChange: function(selectedDates, dateStr, instance) {
                displayTimeline(dateStr);
            }
        });

        // Function to display timeline based on selected date
        function displayTimeline(selectedDate) {
            // Get all timeline items
            const timelineItems = document.querySelectorAll('.timeline-item');
            let visibleItems = 0;

            timelineItems.forEach(item => {
                const itemDate = item.getAttribute('data-date').split(' ')[0]; // Get only the date part
                if (itemDate === selectedDate) {
                    item.style.display = 'block'; // Show item
                    visibleItems++;
                } else {
                    item.style.display = 'none'; // Hide item
                }
            });

            if (visibleItems === 0) {
                document.getElementById('timeline-container').innerHTML = '<p>No data available for the selected date.</p>';
            }
        }

        // Function to view details in a modal
        function viewDetails(button) {
            const item = button.closest('.timeline-item');
            document.getElementById('error-date').innerText = item.getAttribute('data-date');
            document.getElementById('error-type').innerText = item.getAttribute('data-type');
            document.getElementById('error-source').innerText = item.getAttribute('data-source');
            document.getElementById('error-authid').innerText = item.getAttribute('data-authid');
            document.getElementById('error-code').innerText = item.getAttribute('data-code');
            document.getElementById('error-description').innerText = item.getAttribute('data-description');

            // Show the modal
            var myModal = new bootstrap.Modal(document.getElementById('errorModal'));
            myModal.show();
        }
    </script>
</body>
</html>


<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Real-Time Authorization with Date Selection</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/flatpickr@4.6.9/dist/flatpickr.min.css" rel="stylesheet">
    <style>
        /* Calendar and Timeline Styles */
        .timeline {
            position: relative;
            padding: 20px;
            border-left: 3px solid #007bff;
            margin-top: 20px;
        }
        .timeline-item {
            margin-bottom: 20px;
            padding-left: 20px;
            position: relative;
        }
        .timeline-item::before {
            content: '';
            position: absolute;
            left: -11px;
            top: 0;
            width: 20px;
            height: 20px;
            background-color: #007bff;
            border-radius: 50%;
            z-index: 1;
        }
        .timeline-date {
            font-weight: bold;
            margin-bottom: 5px;
            color: #007bff;
        }
        .timeline-content {
            padding: 10px 15px;
            background: #f8f9fa;
            border-radius: 5px;
            border: 1px solid #e0e0e0;
        }
    </style>
</head>
<body>
    <div class="container mt-5">
        <!-- Calendar Selector -->
        <div class="mb-4">
            <h4>Select Date to View Authorization Errors</h4>
            <input type="text" id="date-picker" class="form-control" placeholder="Select a Date">
        </div>

        <!-- Timeline Section -->
        <div id="timeline-container">
            <!-- Timeline content will be dynamically updated here based on the selected date -->
        </div>
    </div>

    <!-- Modal (for error details) -->
    <div class="modal fade" id="errorModal" tabindex="-1" aria-labelledby="errorModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="errorModalLabel">Error Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p><strong>Date:</strong> <span id="error-date"></span></p>
                    <p><strong>Type:</strong> <span id="error-type"></span></p>
                    <p><strong>Source:</strong> <span id="error-source"></span></p>
                    <p><strong>AuthID Received:</strong> <span id="error-authid"></span></p>
                    <p><strong>ErrorCode:</strong> <span id="error-code"></span></p>
                    <p><strong>ErrorDescription:</strong> <span id="error-description"></span></p>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/flatpickr@4.6.9/dist/flatpickr.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // Sample error data
        const errorData = [
            {
                date: '7/19/2023 6:12:05 AM',
                type: 'Error',
                source: 'AuthExport_Realtime_CIGNA_GTM',
                authIdReceived: 'EVIRT002',
                errorCode: 'EVIRT002',
                errorDescription: 'General Exception: Failure',
            },
            {
                date: '8/11/2023 12:47:39 PM',
                type: 'Error',
                source: 'AuthExport_Realtime_CIGNA_GTM',
                authIdReceived: 'EVIRT002',
                errorCode: 'EVIRT002',
                errorDescription: 'General Exception: Failure',
            },
            {
                date: '8/16/2023 3:46:30 PM',
                type: 'Error',
                source: 'AuthExport_Realtime_CIGNA_GTM',
                authIdReceived: 'EVIRT001',
                errorCode: 'EVIRT001',
                errorDescription: 'An error occurred while processing the request',
            }
        ];

        // Initialize the date picker
        flatpickr("#date-picker", {
            dateFormat: "m/d/Y",
            onChange: function(selectedDates, dateStr, instance) {
                displayTimeline(dateStr);
            }
        });

        // Function to display timeline based on selected date
        function displayTimeline(selectedDate) {
            // Filter errors by the selected date
            const filteredData = errorData.filter(error => {
                return error.date.startsWith(selectedDate);
            });

            let timelineHTML = '';

            if (filteredData.length > 0) {
                filteredData.forEach(error => {
                    timelineHTML += `
                    <div class="timeline-item">
                        <div class="timeline-date">${error.date}</div>
                        <div class="timeline-content">
                            <h5><span class="badge bg-danger">${error.type}</span></h5>
                            <p><strong>Source:</strong> ${error.source}</p>
                            <p><strong>AuthID Received:</strong> ${error.authIdReceived}</p>
                            <p><strong>ErrorCode:</strong> ${error.errorCode}</p>
                            <p><strong>ErrorDescription:</strong> ${error.errorDescription}</p>
                            <button class="btn btn-primary btn-sm" onclick="viewDetails('${error.date}', '${error.type}', '${error.source}', '${error.authIdReceived}', '${error.errorCode}', '${error.errorDescription}')">View Details</button>
                        </div>
                    </div>
                    `;
                });
            } else {
                timelineHTML = '<p>No data available for the selected date.</p>';
            }

            // Update the timeline container
            document.getElementById('timeline-container').innerHTML = timelineHTML;
        }

        // Function to view details in a modal
        function viewDetails(date, type, source, authIdReceived, errorCode, errorDescription) {
            document.getElementById('error-date').innerText = date;
            document.getElementById('error-type').innerText = type;
            document.getElementById('error-source').innerText = source;
            document.getElementById('error-authid').innerText = authIdReceived;
            document.getElementById('error-code').innerText = errorCode;
            document.getElementById('error-description').innerText = errorDescription;

            // Show the modal
            var myModal = new bootstrap.Modal(document.getElementById('errorModal'));
            myModal.show();
        }
    </script>
</body>
</html>





<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Authorization Responses UI</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        /* Card Layout */
        .card {
            margin-bottom: 20px;
        }

        /* Timeline Design */
        .timeline {
            position: relative;
            padding: 20px;
            border-left: 3px solid #007bff;
        }
        .timeline-item {
            margin-bottom: 20px;
            padding-left: 20px;
            position: relative;
        }
        .timeline-item::before {
            content: '';
            position: absolute;
            left: -11px;
            top: 0;
            width: 20px;
            height: 20px;
            background-color: #007bff;
            border-radius: 50%;
            z-index: 1;
        }
        .timeline-date {
            font-weight: bold;
            margin-bottom: 5px;
            color: #007bff;
        }
        .timeline-content {
            padding: 10px 15px;
            background: #f8f9fa;
            border-radius: 5px;
            border: 1px solid #e0e0e0;
        }
    </style>
</head>
<body>
    <div class="container mt-5">
        <!-- Card-Based Layout -->
        <div class="row">
            <div class="col-md-4 mb-3">
                <div class="card border-primary">
                    <div class="card-header bg-primary text-white">
                        <strong>Date:</strong> 7/19/2023 6:12:05 AM
                    </div>
                    <div class="card-body">
                        <p><strong>Type:</strong> Error</p>
                        <p><strong>Source:</strong> AuthExport_Realtime_CIGNA_GTM</p>
                        <p><strong>AuthID Received:</strong> EVIRT002</p>
                        <p><strong>ErrorCode:</strong> EVIRT002</p>
                        <p><strong>ErrorDescription:</strong> General Exception: Failure</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-3">
                <div class="card border-danger">
                    <div class="card-header bg-danger text-white">
                        <strong>Date:</strong> 8/11/2023 12:47:39 PM
                    </div>
                    <div class="card-body">
                        <p><strong>Type:</strong> Error</p>
                        <p><strong>Source:</strong> AuthExport_Realtime_CIGNA_GTM</p>
                        <p><strong>AuthID Received:</strong> EVIRT002</p>
                        <p><strong>ErrorCode:</strong> EVIRT002</p>
                        <p><strong>ErrorDescription:</strong> General Exception: Failure</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Timeline Design -->
        <div class="timeline">
            <div class="timeline-item">
                <div class="timeline-date">7/19/2023 6:12:05 AM</div>
                <div class="timeline-content">
                    <h5><span class="badge bg-danger">Error</span></h5>
                    <p><strong>Source:</strong> AuthExport_Realtime_CIGNA_GTM</p>
                    <p><strong>AuthID Received:</strong> EVIRT002</p>
                    <p><strong>ErrorCode:</strong> EVIRT002</p>
                    <p><strong>ErrorDescription:</strong> General Exception: Failure</p>
                </div>
            </div>
            <div class="timeline-item">
                <div class="timeline-date">8/11/2023 12:47:39 PM</div>
                <div class="timeline-content">
                    <h5><span class="badge bg-danger">Error</span></h5>
                    <p><strong>Source:</strong> AuthExport_Realtime_CIGNA_GTM</p>
                    <p><strong>AuthID Received:</strong> EVIRT002</p>
                    <p><strong>ErrorCode:</strong> EVIRT002</p>
                    <p><strong>ErrorDescription:</strong> General Exception: Failure</p>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>







using System;

namespace Singleton_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            ApplicationState state = ApplicationState.GetState();
            state.LoginId = "Gangadhar";
            state.RoleId = "eedhala";

            ApplicationState st2 = ApplicationState.GetState();
            var lId = st2.LoginId;
            var rId = st2.RoleId;
            var b = (state == st2).ToString();

            Console.WriteLine(lId + "\n" + rId + "\n"+ b);
        }

        public class ApplicationState
        {
            private static ApplicationState Instance = null;            
            public string LoginId
            {
                get;
                set;
            }
            public string RoleId
            {
                get;
                set;
            }

            public ApplicationState()
            {

            }

            private static object lockThis = new object();

            public static ApplicationState GetState()
            {
                lock (lockThis)
                {
                    if(ApplicationState.Instance == null)
                    {
                        Instance = new ApplicationState();
                    }
                }
                return Instance;
            }

        }
    }
}
