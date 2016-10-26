(function(){

	// navigation	
	if(document.querySelector('#js-accueil') !== null){

		var el = document.querySelector('.nav-accueil');
		el.classList.add('nav-accueil--active');
	}
	else if(document.querySelector('#js-prevention') !== null){

		el = document.querySelector('.nav-prevention');
		el.classList.add('nav-prevention--active');
	}
	else if(document.querySelector('#js-recherche') !== null){

		el = document.querySelector('.nav-recherche');
		el.classList.add('nav-recherche--active');
	}
	else if(document.querySelector('#js-formation') !== null){

		el = document.querySelector('.nav-formation');
		el.classList.add('nav-formation--active');
	} 
	else if(document.querySelector('#js-quiSommesNous') !== null){

		el = document.querySelector('.nav-adhesion');
		el.classList.add('nav-adhesion--active');
	}
})();