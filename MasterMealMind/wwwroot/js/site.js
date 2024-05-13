function toggleCheckbox(id) {
	var checkbox = document.getElementById(id);
	checkbox.checked = !checkbox.checked;
}

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

	$(document).ready(function() {
		// När knappen klickas, gör ingredienserna redigerbara
		$("#editIngredientsBtn").click(function () {
			$(".ingredient").attr("contenteditable", "true");
			// Visa en annan knapp för att spara ändringarna
			$(this).hide();
			$("#saveIngredientsBtn").show();
		});

	// Funktion för att spara ändringar och avsluta redigeringen
	$("#saveIngredientsBtn").click(function() {
		$(".ingredient").removeAttr("contenteditable");
	// Göm spara-knappen och visa redigeringsknappen igen
	$(this).hide();
	$("#editIngredientsBtn").show();
            // Spara ändringarna, du kan här anropa en funktion eller göra en AJAX-förfrågan till servern för att spara ändringarna
        });
    });

