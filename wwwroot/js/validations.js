function isValidEmail() {
    var url = new URL(location.href);
    var email = document.getElementById('email');
     var filter =new RegExp('/\S+@\S+\.\S+/');

    if (!filter.test(email.value)) {
    alert('Please provide a valid email address');
    return false;
 }
}