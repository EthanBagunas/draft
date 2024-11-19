function populateTimeSlots() {
    const timeIn = document.getElementById('timeIn');
    const timeOut = document.getElementById('timeOut');
    timeIn.innerHTML = '<option value="">Select start time</option>';
    timeOut.innerHTML = '<option value="">Select end time</option>';

    // Generate time slots from 8 AM to 9 PM
    for (let hour = 8; hour <= 21; hour++) {
        const time = `${hour.toString().padStart(2, '0')}:00`;
        timeIn.add(new Option(time, time));
        timeOut.add(new Option(time, time));
    }
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