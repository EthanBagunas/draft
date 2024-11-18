function addRoom(event) {
    event.preventDefault();
    const roomData = {
        roomname: document.getElementById('roomName').value,
        maxCapacity: document.getElementById('capacity').value
    };

    $.ajax({
        url: '/Home/NewRoom',
        type: 'POST',
        data: roomData,
        success: function(response) {
            console.log('Room added successfully:', response);
            alert('Room added successfully!');
            closeModal();
            location.reload();
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