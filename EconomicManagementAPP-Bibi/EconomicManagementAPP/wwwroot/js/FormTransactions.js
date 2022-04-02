function initializeFormTransactions(getCategoriesURL) {
    $("#OperationTypeId").change(async function () {
        const valueSelected = $(this).val();

        const response = await fetch(getCategoriesURL, {
            method: 'POST',
            body: valueSelected,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const json = await response.json();

        const options = json.map(categories => `<option value=${categories.value}>${categories.text}</option>`);
        $("#CategoryId").html(options);
    })
}