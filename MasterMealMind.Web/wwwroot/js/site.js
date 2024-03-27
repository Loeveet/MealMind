
let selectedIds = [];
let checkboxes = document.querySelectorAll('.product-checkbox');
function updateSelectedIds() {
	selectedIds = Array.from(checkboxes)
		.filter(function (checkbox) {
			return checkbox.checked;
		})
		.map(function (checkbox) {
			return checkbox.getAttribute('data-grocery-name');
		});

	// Update the hidden input field with selected IDs
	document.getElementById('selectedGroceryNames').value = selectedIds.join(' ');
}

let allChecked = false;
checkboxes.forEach(function (checkbox) {
	checkbox.addEventListener('change', function () {
		let notAllChecked = false;

		checkboxes.forEach(function (checkbox) {
			if (!checkbox.checked) {
				notAllChecked = true;
			}
		});

		allChecked = !notAllChecked; // Update allChecked based on all checkboxes status
		updateSelectedIds();
	});
});
