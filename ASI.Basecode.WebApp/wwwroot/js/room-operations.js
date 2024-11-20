function addRoom(event) {
    event.preventDefault();
    const roomData = {
        roomname: document.getElementById('roomName').value,
        maxCapacity: document.getElementById('capacity').value
    };

    $.ajax({
        url: '/Account/NewRoom',
        type: 'POST',
        data: roomData,
        success: function(response) {
            if (response.success) {
                console.log('Room added successfully:', response);
                alert('Room added successfully!');
                closeModal();
                location.reload();
            } else {
                console.error('Error adding room:', response.message);
                alert(response.message || 'Failed to add room');
            }
        },
        error: function(error) {
            console.error('Error adding room:', error);
            alert('Failed to add room. Please check console for details.');
        }
    });
}

function deleteRoom(roomId) {
    if (confirm('Are you sure you want to delete this room?')) {
        $.ajax({
            url: '/Home/DeleteRoom',
            type: 'POST',
            data: { roomId: roomId },
            success: function(response) {
                console.log('Room deleted successfully:', response);
                alert('Room deleted successfully!');
                location.reload();
            },
            error: function(error) {
                console.error('Error deleting room:', error);
                alert('Failed to delete room. Please check console for details.');
            }
        });
    }
}

function editRoom(roomId) {
    // First fetch the room details
    $.ajax({
        url: `/Home/GetRoom/${roomId}`,
        type: 'GET',
        success: function(room) {
            // Populate the modal with room details
            document.getElementById('roomName').value = room.roomname;
            document.getElementById('capacity').value = room.maxCapacity;
            
            // Store the room ID for the update operation
            document.getElementById('addUserForm').dataset.roomId = roomId;
            
            // Open the modal
            openModal();
        },
        error: function(error) {
            console.error('Error fetching room details:', error);
            alert('Failed to fetch room details. Please try again.');
        }
    });
}

function showBookings(roomId) {
    $.ajax({
        url: `/Account/GetBookingsbyRoomid?roomid=${roomId}`,
        type: 'POST',
        success: function(bookings) {
            if (Array.isArray(bookings)) {
                populateBookingsTable(bookings);
                openBookingsModal();
            } else {
                console.error('Invalid response format:', bookings);
                alert('Failed to fetch bookings');
            }
        },
        error: function(error) {
            console.error('Error fetching bookings:', error);
            alert('Failed to fetch bookings');
        }
    });
}

function populateBookingsTable(bookings) {
    const tbody = document.getElementById('bookingsTableBody');
    tbody.innerHTML = '';

    if (bookings.length === 0) {
        const row = document.createElement('tr');
        row.innerHTML = '<td colspan="6" style="text-align: center;">No bookings found</td>';
        tbody.appendChild(row);
        return;
    }

    bookings.forEach(booking => {
        // Only show bookings with status 'RESERVED' or 'VACANT'
        if (booking.status === 'RESERVED' || booking.status === 'VACANT') {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${booking.customer?.custfname || '--'}</td>
                <td>${booking.customer?.contact || '--'}</td>
                <td>${booking.bookingDate ? new Date(booking.bookingDate).toLocaleDateString() : '--'}</td>
                <td>${booking.timeIn ? new Date('1970-01-01T' + booking.timeIn).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
                <td>${booking.timeOut ? new Date('1970-01-01T' + booking.timeOut).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
                <td>${booking.status}</td>
            `;
            tbody.appendChild(row);
        }
    });
}

function openBookingsModal() {
    const modal = document.getElementById('bookingsModal');
    if (modal) {
        modal.style.display = 'block';
    } else {
        console.error('Bookings modal not found');
    }
}

function closeBookingsModal() {
    const modal = document.getElementById('bookingsModal');
    if (modal) {
        modal.style.display = 'none';
    }
}

function showAllBookings() {
    $.ajax({
        url: '/Account/GetAllBookings',
        type: 'GET',
        success: function(bookings) {
            if (Array.isArray(bookings)) {
                populateAllBookingsTable(bookings);
                openAllBookingsModal();
            } else {
                console.error('Invalid response format:', bookings);
                alert('Failed to fetch bookings');
            }
        },
        error: function(error) {
            console.error('Error fetching all bookings:', error);
            alert('Failed to fetch bookings');
        }
    });
}

function populateAllBookingsTable(bookings) {
    const tbody = document.getElementById('allBookingsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';

    if (!bookings || bookings.length === 0) {
        tbody.innerHTML = '<tr><td colspan="8" style="text-align: center;">No bookings found</td></tr>';
        return;
    }

    // Sort bookings by Room ID
    const sortedBookings = bookings.sort((a, b) => a.roomId - b.roomId);

    sortedBookings.forEach(booking => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${booking.room?.roomNumber || '--'}</td>
            <td>${booking.room?.roomname || '--'}</td>
            <td>${booking.customer?.custfname || '--'}</td>
            <td>${booking.customer?.contact || '--'}</td>
            <td>${booking.bookingDate ? new Date(booking.bookingDate).toLocaleDateString() : '--'}</td>
            <td>${booking.timeIn ? new Date('1970-01-01T' + booking.timeIn).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
            <td>${booking.timeOut ? new Date('1970-01-01T' + booking.timeOut).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
            <td>${booking.status}</td>
        `;
        tbody.appendChild(row);
    });
}

function openAllBookingsModal() {
    const modal = document.getElementById('allBookingsModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

function closeAllBookingsModal() {
    const modal = document.getElementById('allBookingsModal');
    if (modal) {
        modal.style.display = 'none';
    }
}

// Update the window click handler to handle both modals
window.onclick = function(event) {
    const bookingsModal = document.getElementById('bookingsModal');
    const allBookingsModal = document.getElementById('allBookingsModal');
    
    if (event.target === bookingsModal) {
        closeBookingsModal();
    }
    if (event.target === allBookingsModal) {
        closeAllBookingsModal();
    }
} 