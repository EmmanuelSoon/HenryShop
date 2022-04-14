window.onload = function () {
    let adding = document.getElementsByClassName("btn-outline-success");
    let minus = document.getElementsByClassName("btn-outline-danger");
    let remove = document.getElementsByClassName("btn-danger");
    alert(adding.length);

    //for (let i = 0; i < adding.length; i++) {
    //    adding[i].addEventListener('click', Addclick);
    //}

    //for (let i = 0; i < minus.length; i++) {
    //    minus[i].addEventListener('click', Minusclick);
    //}

    //for (let i = 0; i < remove.length; i++) {
    //    remove[i].addEventListener('click', Removeclick);
    //}

    // combine above three codes
    for (let i = 0; i < adding.length; i++) {
        adding[i].addEventListener('click', Addclick);
        minus[i].addEventListener('click', Minusclick);
        remove[i].addEventListener('click', Removeclick);
    }

    function Addclick(event) {
        PlusToCart(event.target.value);
    }

    function Minusclick(event) {
        if (event.target.getAttribute('data-value') <= 0) {
            RemoveFromCart(event.target.getAttribute('value'));
        }
        else {
            MinusFromCart(event.target.getAttribute('value'));
        }
    }

    function Removeclick(event) {
        RemoveFromCart(event.target.value);       
    }
}





function PlusToCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/CheckOut/PlusToCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                // convert server's JSON string to a JavaScript object
                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    window.location.reload(true);
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}

function MinusFromCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/CheckOut/MinusFromCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    window.location.reload(true);
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}


function RemoveFromCart(id) {
    if (confirm("Remove item from Cart?")) {
        let xhr = new XMLHttpRequest();

        xhr.open("POST", "/CheckOut/RemoveFromCart");

        xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

        xhr.onreadystatechange = function () {
            if (this.readyState == XMLHttpRequest.DONE) {
                if (this.status == 200) {
                    let data = JSON.parse(this.responseText);


                    if (data.status == "success") {
                        window.location.reload(true);
                    }
                }
            }
        }

        let req = { "Id": id }

        xhr.send(JSON.stringify(req));
    }
}