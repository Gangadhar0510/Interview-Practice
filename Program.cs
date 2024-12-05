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
