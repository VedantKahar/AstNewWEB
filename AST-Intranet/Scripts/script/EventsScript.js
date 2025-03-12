    $(document).ready(function () {
        const apiKey = 'PFXdUFwPMihLk1IzaFIng3tUuzhftAHd'; // API Key
        const countryCode = 'IN'; // Country code for India
        const year = 2025; // Year for fetching holidays

        let currentEvent = null; // Track the event currently being edited or deleted

        // Fetch holidays from Calendarific API
        function fetchIndianHolidays() {
            //localStorage.removeItem('holidays');  // Remove old holidays to prevent duplicates
            //localStorage.removeItem('events');    // Remove user-added events

            const url = `https://calendarific.com/api/v2/holidays?api_key=${apiKey}&country=${countryCode}&year=${year}`;

            fetch(url)
                .then(response => response.json())
                .then(data => {
                    if (data.response && data.response.holidays) {
                        const holidays = data.response.holidays;

                        // Map holidays to events format
                        const events = holidays.map(holiday => {
                            return {
                                title: holiday.name,
                                start: moment(holiday.date.iso).format(),
                                description: holiday.description,
                                allDay: true
                            };
                        });

                        saveHolidaysToLocalStorage(events);  // Store holidays in localStorage
                        addHolidaysToCalendar(events);  // Add holidays to calendar
                    } else {
                        console.error("Error fetching holidays:", data);
                    }
                })
                .catch(error => {
                    console.error("Failed to fetch holidays from the API:", error);
                });
        }

        // Function to add holidays to the calendar
        function addHolidaysToCalendar(holidays) {
            console.log("Adding holidays to calendar:", holidays);

            const storedHolidays = getStoredEvents();  // Get stored events
            const newHolidays = holidays.filter(holiday => {
                // Prevent adding duplicate holidays
                return !storedHolidays.some(stored => stored.start === holiday.start && stored.title === holiday.title);
            });

            if (newHolidays.length > 0) {
                $('#calendar').fullCalendar('addEventSource', newHolidays);
            }
        }

        // Save holidays in localStorage
        function saveHolidaysToLocalStorage(holidays) {
            const storedHolidays = JSON.parse(localStorage.getItem('holidays')) || [];
            const allHolidays = storedHolidays.concat(holidays);

            const uniqueHolidays = allHolidays.filter((value, index, self) =>
                index === self.findIndex((t) => (
                    t.start === value.start && t.title === value.title
                ))
            );

            localStorage.setItem('holidays', JSON.stringify(uniqueHolidays));
        }

        // Initialize FullCalendar
        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            events: function (start, end, timezone, callback) {
                const events = getStoredEvents();  // Get events from localStorage
                callback(events);  // Display events on the calendar
            },
            dayClick: function (date, jsEvent, view) {
                $('#event-date').val(date.format('YYYY-MM-DD'));  // Set the date in the form
                $('#event-form').show();  // Show the event form
            },
            eventClick: function (event, jsEvent, view) {
                currentEvent = event;
                showEventDetails(event);  // Show event details modal
            }
        });

        // Get events stored in localStorage
        function getStoredEvents() {
            const storedEvents = JSON.parse(localStorage.getItem('events')) || [];
            return storedEvents;
        }

        // Show event details in a modal
        function showEventDetails(event) {
            $('#event-details-title').text(event.title);
            $('#event-details-time').text(event.start);
            $('#event-details-description').text(event.description);
            $('#event-details-modal').show();
        }

        // Add a new event
        function addEvent() {
            const eventName = $('#event-name').val();
            const eventDate = $('#event-date').val();
            const eventTime = $('#event-time').val();
            const eventDescription = $('#event-description').val();
            const isFullDay = $('#event-full-day').prop('checked');

            if (eventName && eventDate && eventDescription) {
                let eventStart = eventDate;

                if (!isFullDay && eventTime) {
                    eventStart = `${eventDate} ${eventTime}`;
                }

                const eventId = new Date().getTime(); // Unique event ID

                const event = {
                    id: eventId,
                    title: eventName,
                    start: eventStart,
                    description: eventDescription,
                    allDay: isFullDay
                };

                $('#calendar').fullCalendar('renderEvent', event);  // Add event to FullCalendar
                saveEventToLocalStorage(event);  // Save event to localStorage
                closeForm();  // Close the event form
            } else {
                alert('Please fill in all fields');
            }
        }

        // Save event to localStorage
        function saveEventToLocalStorage(event) {
            let events = JSON.parse(localStorage.getItem('events')) || [];
            events = events.filter(e => e.id !== event.id);  // Remove existing event if it exists
            events.push(event);  // Add the new event
            localStorage.setItem('events', JSON.stringify(events));

            $('#calendar').fullCalendar('removeEvents');  // Clear current events
            $('#calendar').fullCalendar('addEventSource', getStoredEvents());  // Re-add events from localStorage
        }

        // Close event form
        function closeForm() {
            $('#event-form').hide();
        }

        // Delete event
        function deleteEvent() {
            if (currentEvent) {
                $('#calendar').fullCalendar('removeEvents', currentEvent.id);  // Remove from calendar
                let events = JSON.parse(localStorage.getItem('events')) || [];
                events = events.filter(event => event.id !== currentEvent.id);  // Remove from localStorage
                localStorage.setItem('events', JSON.stringify(events));

                $('#calendar').fullCalendar('removeEvents');  // Remove all events
                $('#calendar').fullCalendar('addEventSource', getStoredEvents());  // Re-add from localStorage

                closeDetailsModal();  // Close event details modal
            }
        }

        // Function to edit an event
        function editEvent() {
            if (currentEvent) {
                const eventName = $('#event-name').val();
                const eventDate = $('#event-date').val();
                const eventTime = $('#event-time').val();
                const eventDescription = $('#event-description').val();
                const isFullDay = $('#event-full-day').prop('checked');

                if (eventName && eventDate && eventDescription) {
                    let eventStart = eventDate;

                    if (!isFullDay && eventTime) {
                        eventStart += ' ' + eventTime;
                    }

                    currentEvent.title = eventName;
                    currentEvent.start = eventStart;
                    currentEvent.description = eventDescription;
                    currentEvent.allDay = isFullDay;

                    $('#calendar').fullCalendar('updateEvent', currentEvent);

                    saveEventToLocalStorage(currentEvent);

                    closeForm();
                } else {
                    alert('Please fill in all fields');
                }
            }
        }

        // Close event details modal
        function closeDetailsModal() {
            $('#event-details-modal').hide();
        }

        // Attach event handlers
        $('#addEventBtn').on('click', addEvent);
        $('#deleteEventBtn').on('click', deleteEvent);
        $('#closeFormBtn').on('click', closeForm);
        $('#closeDetailsModal').on('click', closeDetailsModal);

        // Fetch holidays and add them to the calendar
        fetchIndianHolidays();
    });

    //why added events by users disappear when refresh,and if i add event all other holidays' disappear;
    //please improve functionalities and working of calendar