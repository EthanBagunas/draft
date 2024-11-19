function populateTimeSlots(selectedDate) {
    const timeIn = document.getElementById('timeIn');
    const timeOut = document.getElementById('timeOut');
    const roomId = document.getElementById('roomSelect').value;
    
    // Clear existing options
    timeIn.innerHTML = '<option value="">Select start time</option>';
    timeOut.innerHTML = '<option value="">Select end time</option>';
    timeOut.disabled = true; // Disable end time until start time is selected

    // Get current date and time
    const now = new Date();
    const selectedDateTime = new Date(selectedDate);
    const isToday = selectedDateTime.toDateString() === now.toDateString();
    
    // Get existing bookings for the selected room and date
    $.get('/Home/GetBookingsForRoom', { 
        roomId: roomId,
        date: selectedDate 
    }, function(bookings) {
        // Generate available time slots
        for (let hour = 8; hour <= 21; hour++) {
            const timeSlot = `${hour.toString().padStart(2, '0')}:00`;
            
            // For today's bookings, don't show past times
            if (isToday && hour <= now.getHours()) {
                continue;
            }

            // Check if this time slot is already booked
            const isBooked = bookings.some(booking => {
                const bookingStart = new Date(`2000/01/01 ${booking.timeIn}`);
                const bookingEnd = new Date(`2000/01/01 ${booking.timeOut}`);
                const slotTime = new Date(`2000/01/01 ${timeSlot}`);
                return slotTime >= bookingStart && slotTime < bookingEnd;
            });

            if (!isBooked) {
                timeIn.add(new Option(timeSlot, timeSlot));
            }
        }
    });
}

function updateEndTimeOptions() {
    const timeIn = document.getElementById('timeIn');
    const timeOut = document.getElementById('timeOut');
    const selectedStartTime = timeIn.value;

    if (!selectedStartTime) {
        timeOut.disabled = true;
        timeOut.innerHTML = '<option value="">Select end time</option>';
        return;
    }

    timeOut.disabled = false;
    timeOut.innerHTML = '<option value="">Select end time</option>';

    // Convert selected start time to Date object for comparison
    const startHour = parseInt(selectedStartTime.split(':')[0]);
    
    // Populate end time options (must be at least 1 hour after start time)
    for (let hour = startHour + 1; hour <= 21; hour++) {
        const timeSlot = `${hour.toString().padStart(2, '0')}:00`;
        
        // Check if this end time slot is available
        const isAvailable = !isTimeSlotBooked(timeSlot);
        if (isAvailable) {
            timeOut.add(new Option(timeSlot, timeSlot));
        }
    }
}

function isTimeSlotBooked(timeSlot) {
    const roomId = document.getElementById('roomSelect').value;
    const selectedDate = document.getElementById('bookingDate').value;
    let isBooked = false;

    // Synchronous AJAX call to check if time slot is booked
    $.ajax({
        url: '/Home/CheckTimeSlotAvailability',
        type: 'GET',
        async: false,
        data: {
            roomId: roomId,
            date: selectedDate,
            time: timeSlot
        },
        success: function(response) {
            isBooked = response.isBooked;
        }
    });

    return isBooked;
}

function populateRooms() {
    $.get('/Home/GetAllRooms', function(response) {
        const select = document.getElementById('roomSelect');
        select.innerHTML = '<option value="">Select a room</option>';
        response.forEach(room => {
            // Only add rooms that are ACTIVE and not currently booked
            if (room.status === 'ACTIVE') {
                select.add(new Option(room.roomname, room.id));
            }
        });
    });
}

function setMinDate() {
    const dateInput = document.getElementById('bookingDate');
    const today = new Date();
    
    // Format today's date as YYYY-MM-DD
    const formattedDate = today.toISOString().split('T')[0];
    dateInput.min = formattedDate;
    
    // Set default value to today
    dateInput.value = formattedDate;
}

function validateTimeSelection() {
    const timeIn = document.getElementById('timeIn').value;
    const timeOut = document.getElementById('timeOut').value;
    
    if (timeIn && timeOut) {
        const startTime = new Date(`2000/01/01 ${timeIn}`);
        const endTime = new Date(`2000/01/01 ${timeOut}`);
        
        if (endTime <= startTime) {
            alert('End time must be after start time');
            document.getElementById('timeOut').value = '';
            return false;
        }

        // Validate business hours (8 AM - 9 PM)
        const hour = startTime.getHours();
        if (hour < 8 || hour > 21) {
            alert('Bookings must be between 8 AM and 9 PM');
            return false;
        }
    }
    return true;
}

function addBooking(event) {
    event.preventDefault();
    
    if (!validateTimeSelection()) {
        return;
    }

    const formData = {
        roomId: document.getElementById('roomSelect').value,
        bookingDate: document.getElementById('bookingDate').value,
        timeIn: document.getElementById('timeIn').value,
        timeOut: document.getElementById('timeOut').value,
        guestName: document.getElementById('guestName').value,
        contactNumber: document.getElementById('contactNumber').value
    };

    // Additional validation
    if (!formData.roomId) {
        alert('Please select a room');
        return;
    }

    if (!formData.guestName || !formData.contactNumber) {
        alert('Please fill in all guest information');
        return;
    }

    $.ajax({
        url: '/Home/NewBook',
        type: 'POST',
        data: formData,
        success: function(response) {
            if (response.success) {
                alert('Booking added successfully!');
                closeModal();
                location.reload();
            } else {
                alert(response.message || 'Failed to add booking');
            }
        },
        error: function(error) {
            alert(error.responseJSON?.message || 'Failed to add booking');
        }
    });
}

// Initialize modal when opened
function openModal() {
    document.getElementById('myModal').style.display = 'flex';
    populateTimeSlots();
    setMinDate();
    populateRooms();
    
    // Add event listeners
    document.getElementById('timeIn').addEventListener('change', validateTimeSelection);
    document.getElementById('timeOut').addEventListener('change', validateTimeSelection);
}

function closeModal() {
    document.getElementById('myModal').style.display = 'none';
    document.getElementById('bookingForm').reset();
}

// Event listeners
document.getElementById('bookingDate').addEventListener('change', function() {
    if (document.getElementById('roomSelect').value) {
        populateTimeSlots(this.value);
    }
});

document.getElementById('roomSelect').addEventListener('change', function() {
    if (document.getElementById('bookingDate').value) {
        populateTimeSlots(document.getElementById('bookingDate').value);
    }
});

document.getElementById('timeIn').addEventListener('change', updateEndTimeOptions);