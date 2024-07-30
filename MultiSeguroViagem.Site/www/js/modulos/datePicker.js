function datepickerSetup() {
    var date = new Date();

    $('.datepicker').datetimepicker({
        useCurrent: false,
        ignoreReadonly: true,
        format: 'DD/MM/YYYY',
        locale: 'pt-br',
        minDate: new Date(date.getFullYear(), date.getMonth(), date.getDate()),
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-chevron-up",
            down: "fa fa-chevron-down",
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-screenshot',
            clear: 'fa fa-trash',
            close: 'fa fa-remove',
            inline: true
        }
    });

    $('.selectpicker').selectpicker({
        dropupAuto: false
    });
};

function datepickerSetaMinMaxDate(input1, input2) {
    $(input1).on("dp.change", function (e) {
        $(input2).data("DateTimePicker").minDate(e.date);
    });
    $(input2).on("dp.change", function (e) {
        $(input1).data("DateTimePicker").maxDate(e.date);
    });
}