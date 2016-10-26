(function(){
    
    var el = document.querySelector('.header__menu');
    el.addEventListener('click', function(e){
        e.preventDefault();
        var elMenu = document.querySelector('body');
        elMenu.classList.toggle('width-sidebar');        
    })
    
    el = document.querySelector('.site-cache');
    el.addEventListener('click', function(e){
        //e.preventDefault();
        document.querySelector('body')
            .classList.remove('width-sidebar');        
    })
    
    el = document.querySelector('.nav');
    el.addEventListener('click', function(e){
        //e.preventDefault();
        document.querySelector('body')
            .classList.remove('width-sidebar');        
    })
})();