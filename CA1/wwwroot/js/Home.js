window.onload = function () {

    let add = document.getElementsByClassName("btn-outline-primary");


    for (let i = 0; i < add.length; i++) {
        add[i].addEventListener('click', Addclick);
    }


    function Addclick(event) {
        AddToCart(event.target.value);
    }

}



function AddToCart(id) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Search/AddToCart");

    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {

                let data = JSON.parse(this.responseText);


                if (data.status == "success") {
                    alert(data.name + " Added to cart");
                }
            }
        }
    }

    let req = { "Id": id }

    xhr.send(JSON.stringify(req));
}