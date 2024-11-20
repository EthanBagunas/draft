
function openModal() {
    document.getElementById("myModal").style.display = "block";
}

// Function to close the modal
function closeModal() {
    document.getElementById("myModal").style.display = "none";
}


// Close the modal when clicking outside of it
window.onclick = function(event) {
    const modal = document.getElementById("myModal");
    if (event.target == modal) {
        closeModal();
    }
}
window.onclick = function(event) {
    const modal = document.getElementById("editModal");
    if (event.target == modal) {
        closeEditModal();
    }
}



