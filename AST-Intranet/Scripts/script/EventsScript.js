$(document).ready(function () {
    const apiKey = 'PFXdUFwPMihLk1IzaFIng3tUuzhftAHd'; // API Key
    const countryCode = 'IN'; // Country code for India
    const year = 2025; // Year for fetching holidays

    let currentEvent = null; // Track the event currently being edited or deleted

    function fetchIndianHolidays() {
        // Clear existing holidays from localStorage to avoid mixing old and new data
        localStorage.removeItem('holidays');  // Remove old holidays to prevent duplicates
        localStorage.removeItem('events');    // Remove user-added events

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

                    // Save holidays to localStorage (Now it will store holidays for India only)
                    saveHolidaysToLocalStorage(events);

                    // Add holidays to the calendar
                    addHolidaysToCalendar(events);
                } else {
                    console.error("Error fetching holidays:", data);
                }
            })
            .catch(error => {
                console.error("Failed to fetch holidays from the API:", error);
            });
    }

    function addHolidaysToCalendar(holidays) {
        console.log("Adding holidays to calendar:", holidays);

        // Add events to FullCalendar only once
        const storedHolidays = getStoredEvents();
        const newHolidays = holidays.filter(holiday => {
            // Check if holiday is already in the calendar to avoid duplication
            return !storedHolidays.some(stored => stored.start === holiday.start && stored.title === holiday.title);
        });

        if (newHolidays.length > 0) {
            $('#calendar').fullCalendar('addEventSource', newHolidays);
        }
    }

    function saveHolidaysToLocalStorage(holidays) {  //save holiday to browser's local storage
        // Get the current holidays from localStorage
        const storedHolidays = JSON.parse(localStorage.getItem('holidays')) || [];

        // Combine the new holidays with the stored ones
        const allHolidays = storedHolidays.concat(holidays);

        // Remove duplicates based on event 'start' date and 'title'
        const uniqueHolidays = allHolidays.filter((value, index, self) =>
            index === self.findIndex((t) => (
                t.start === value.start && t.title === value.title
            ))
        );

        // Save back the unique holidays to localStorage
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
            const events = getStoredEvents();  // Get stored events from localStorage
            callback(events);  // Pass them to the calendar without re-adding them elsewhere
        },

        dayClick: function (date, jsEvent, view) {
            // Handle day click
        },
        eventClick: function (event, jsEvent, view) {
            // Set currentEvent to the clicked event
            currentEvent = event;
            showEventDetails(event); // Show event details modal
        },
        viewRender: function (view, element) {
            // Re-add events from localStorage after view is rendered (only once, no duplicates)
            //$('#calendar').fullCalendar('removeEvents');
            //$('#calendar').fullCalendar('addEventSource', getStoredEvents());
        }
    });

    // Function to get stored events from localStorage and retrieve
    function getStoredEvents() {
        const storedHolidays = JSON.parse(localStorage.getItem('holidays')) || [];
        return storedHolidays;
    }

    // Function to show event details in a modal
    function showEventDetails(event) {
        $('#event-details-title').text(event.title);
        $('#event-details-time').text(event.start);
        $('#event-details-description').text(event.description);
        $('#event-details-modal').show();
    }

    // Function to close the event form
    function closeForm() {
        $('#event-form').hide();
    }

    function addEvent() {
        const eventName = $('#event-name').val();
        const eventDate = $('#event-date').val();
        const eventTime = $('#event-time').val();
        const eventDescription = $('#event-description').val();
        const isFullDay = $('#event-full-day').prop('checked');

        console.log('Event Name:', eventName);
        console.log('Event Date:', eventDate);
        console.log('Event Time:', eventTime);
        console.log('Event Description:', eventDescription);
        console.log('Is Full Day:', isFullDay);

        if (eventName && eventDate && eventDescription) {
            let eventStart = eventDate;
            if (!isFullDay && eventTime) {
                eventStart += ' ' + eventTime;
            }

            const eventId = new Date().getTime(); // Unique event ID

            const event = {
                id: eventId,
                title: eventName,
                start: eventStart,
                description: eventDescription,
                allDay: isFullDay
            };

            // Add event to FullCalendar
            $('#calendar').fullCalendar('renderEvent', event);
            saveEventToLocalStorage(event);
            closeForm(); // Close the event form
        } else {
            alert('Please fill in all fields');
        }
    }

    // Function to save event to localStorage
    function saveEventToLocalStorage(event) {
        let events = JSON.parse(localStorage.getItem('events')) || [];
        events = events.filter(e => e.id !== event.id); // Prevent duplicate events
        events.push(event); // Add the new event
        localStorage.setItem('events', JSON.stringify(events));

        // Re-render the events in all views (month, week, day)
        $('#calendar').fullCalendar('removeEvents');
        $('#calendar').fullCalendar('addEventSource', getStoredEvents());
    }

    // Function to delete an event
    function deleteEvent() {
        if (currentEvent) {
            // Remove the event from FullCalendar
            $('#calendar').fullCalendar('removeEvents', currentEvent.id);

            // Remove the event from localStorage
            let events = JSON.parse(localStorage.getItem('events')) || [];
            events = events.filter(event => event.id !== currentEvent.id); // Remove the deleted event
            localStorage.setItem('events', JSON.stringify(events));

            // Re-render the events in all views (month, week, day)
            $('#calendar').fullCalendar('removeEvents');
            $('#calendar').fullCalendar('addEventSource', getStoredEvents());

            closeDetailsModal(); // Close the event details modal
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

    // Function to close the event details modal
    function closeDetailsModal() {
        $('#event-details-modal').hide();
    }

    // Attach event handlers
    $('#addEventBtn').on('click', addEvent);
    $('#editEventBtn').on('click', editEvent);
    $('#closeFormBtn').on('click', closeForm);
    $('#closeDetailsModal').on('click', closeDetailsModal);
    $('#deleteEventBtn').on('click', deleteEvent);

    // Fetch holidays and festivals and add them to the calendar
    fetchIndianHolidays();
});