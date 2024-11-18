function populateTimeSlots() {
    const timeIn = document.getElementById('timeIn');
    const timeOut = document.getElementById('timeOut');
    const startHour = 8;
    const endHour = 21;

    for (let i = startHour; i <= endHour; i++) {
        const time = `${i.toString().padStart(2, '0')}:00`;
        timeIn.add(new Option(time, time));
        timeOut.add(new Option(time, time));
    }
}

function populateRooms() {
    $.get('/Home/GetAllRooms', function(response) {
        const select = document.getElementById('roomSelect');
        response.forEach(room => {
            select.add(new Option(room.roomname, room.id));
        });
    });
}

function addBooking(event) {
    event.preventDefault();
    const formData = {
        roomId: document.getElementById('roomSelect').value,
        bookingDate: document.getElementById('bookingDate').value,
        timeIn: document.getElementById('timeIn').value,
        timeOut: document.getElementById('timeOut').value,
        guestName: document.getElementById('guestName').value,
        contactNumber: document.getElementById('contactNumber').value
    };

    $.ajax({
        url: '/Home/NewBook',
        type: 'POST',
        data: formData,
        success: function(response) {
            alert('Booking added successfully!');
            closeModal();
            location.reload();
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
    populateRooms();
}

function closeModal() {
    document.getElementById('myModal').style.display = 'none';
    document.getElementById('bookingForm').reset();
}