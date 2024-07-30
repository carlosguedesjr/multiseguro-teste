configInputDate = () => {
    

    var div = document.createElement("div");
    div.innerHTML = "<input type='date'>";

    if (div.firstChild.type === "date") {
        return true;
    }
        
    return false;
}