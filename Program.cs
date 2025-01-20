
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
