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

// Add these new functions for homepage search
function filterHomepageTable() {
    const searchInput = document.getElementById('homeSearchInput');
    const filter = searchInput.value.toLowerCase().trim();
    const table = document.querySelector('.dash-table table');
    const rows = table.getElementsByTagName('tr');
    let visibleCount = 0;

    // Skip header row
    for (let i = 1; i < rows.length; i++) {
        const row = rows[i];
        const roomNameCell = row.getElementsByTagName('td')[1];
        let shouldShow = false;

        if (filter === '') {
            shouldShow = true;
        } else if (roomNameCell) {
            const roomName = roomNameCell.textContent || roomNameCell.innerText;
            shouldShow = roomName.toLowerCase().indexOf(filter) > -1;
        }

        if (shouldShow) {
            row.style.display = '';
            visibleCount++;
        } else {
            row.style.display = 'none';
        }
    }

    // Reset to first page when filtering
    currentPage = 1;
    displayTableRows();
}

function updateNoResultsMessage(visibleCount, filter) {
    let noResultsMessage = document.querySelector('.no-results-message');
    
    // Create the message element if it doesn't exist
    if (!noResultsMessage) {
        noResultsMessage = document.createElement('div');
        noResultsMessage.className = 'no-results-message';
        const tableContainer = document.querySelector('.custom-table-container');
        tableContainer.appendChild(noResultsMessage);
    }

    if (visibleCount === 0 && filter !== '') {
        noResultsMessage.style.display = 'block';
        noResultsMessage.textContent = `No rooms found matching "${filter}"`;
    } else {
        noResultsMessage.style.display = 'none';
    }
}

// Add debounce function to improve performance
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Create debounced version of the filter function
const debouncedFilterHomepage = debounce(filterHomepageTable, 300);

// Add event listeners when document is ready
document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.getElementById('homeSearchInput');
    if (searchInput) {
        searchInput.addEventListener('keyup', debouncedFilterHomepage);
        searchInput.addEventListener('search', debouncedFilterHomepage); // For clear button
    }
});

// Pagination variables
let currentPage = 1;
const rowsPerPage = 10;
let totalPages = 1;

function setupPagination(totalItems) {
    totalPages = Math.ceil(totalItems / rowsPerPage);
    
    const paginationContainer = document.querySelector('.custom-pagination-btn');
    if (paginationContainer) {
        let html = `
            <button onclick="goToPage(1)" ${currentPage === 1 ? 'disabled' : ''}>
                <i class="fa-solid fa-angles-left"></i>
            </button>
            <button onclick="goToPage(${currentPage - 1})" ${currentPage === 1 ? 'disabled' : ''}>
                <i class="fa-solid fa-angle-left"></i>
            </button>
        `;

        for (let i = 1; i <= totalPages; i++) {
            html += `<a href="#" onclick="goToPage(${i})" class="${currentPage === i ? 'active' : ''}">${i}</a>`;
        }

        html += `
            <button onclick="goToPage(${currentPage + 1})" ${currentPage === totalPages ? 'disabled' : ''}>
                <i class="fa-solid fa-angle-right"></i>
            </button>
            <button onclick="goToPage(totalPages)" ${currentPage === totalPages ? 'disabled' : ''}>
                <i class="fa-solid fa-angles-right"></i>
            </button>
        `;

        paginationContainer.innerHTML = html;
    }

    const resultsInfo = document.querySelector('.results-info');
    if (resultsInfo) {
        const startItem = (currentPage - 1) * rowsPerPage + 1;
        const endItem = Math.min(currentPage * rowsPerPage, totalItems);
        resultsInfo.textContent = `Showing ${startItem} to ${endItem} out of ${totalItems} results`;
    }
}

function goToPage(page) {
    if (page < 1 || page > totalPages) return;
    currentPage = page;
    displayTableRows();
}

function displayTableRows() {
    const table = document.querySelector('.dash-table table tbody');
    const rows = table.getElementsByTagName('tr');
    const totalItems = rows.length;
    
    const startIndex = (currentPage - 1) * rowsPerPage;
    const endIndex = Math.min(startIndex + rowsPerPage, totalItems);
    
    // Hide all rows first
    for (let i = 0; i < totalItems; i++) {
        rows[i].style.display = 'none';
    }
    
    // Show rows for current page
    for (let i = startIndex; i < endIndex; i++) {
        if (i < totalItems) {
            rows[i].style.display = '';
        }
    }

    // Add empty rows if needed
    const existingEmptyRows = table.querySelectorAll('.empty-row');
    existingEmptyRows.forEach(row => row.remove());

    const visibleRows = endIndex - startIndex;
    if (visibleRows < rowsPerPage) {
        const emptyRowsNeeded = rowsPerPage - visibleRows;
        for (let i = 0; i < emptyRowsNeeded; i++) {
            const emptyRow = document.createElement('tr');
            emptyRow.className = 'empty-row';
            emptyRow.innerHTML = `
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            `;
            table.appendChild(emptyRow);
        }
    }

    setupPagination(totalItems);
}

// Add event listener for DOMContentLoaded
document.addEventListener('DOMContentLoaded', function() {
    displayTableRows();
});