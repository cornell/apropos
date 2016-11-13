set dpc=contrat-formation-dpc
set hors-dpc=contrat-formation-hors-dpc
set salarie=contrat-formation-salarie

cd dist-pdf\markdownForPdf\le-bilan-et-la-reeducation-vocale-le-timbre-en-question & 
wkhtmltopdf %dpc%.html %dpc%.pdf &   
wkhtmltopdf %hors-dpc%.html %hors-dpc%.pdf &  
wkhtmltopdf %salarie%.html %salarie%.pdf &
del *.html &
cd../../..


cd dist-pdf\markdownForPdf\fentes-faciales-et-incompetence-velo-pharyngee & 
wkhtmltopdf %dpc%.html %dpc%.pdf &   
wkhtmltopdf %hors-dpc%.html %hors-dpc%.pdf &  
wkhtmltopdf %salarie%.html %salarie%.pdf &
del *.html &
cd../../..
