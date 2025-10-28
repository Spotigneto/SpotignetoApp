# | Google Colab

https://colab.research.google.com/drive/1iiSSP39SaA02RaQyvDIcbbEiHxFWVo96?usp=sharing

Accedendo a questo link di Google Colab è possibile caricarci il data set proveniente dal file .xlsx e scremarlo così da avere dei file .csv contenenti solo le informazioni utili al fine del database.

Il Layer Bronze si ottiene nel momento in cui lo script verifica che il file non contenga né duplicati né righe nulle.

Si passa quindi al Layer Silver dove c'e stata una scrematura dei dati e conseguente rimozione delle colonne superflue.
Quindi alcuni dati sono stati adattati alle nostre necessità, che esso sia rinominare le colonne di genere e sottogenere oppure le conversioni temporali da millisecondi a secondi e minuti + secondi

Infine per ridurre le dimensioni dei file, nel layer gold è stata fatta una divisione delle canzoni per genere.

L'ultimo blocco di codice permette il download delle 3 entità forti provenienti dal Dataset: Canzone, Artista e Album tramite file .csv
Garantisce inoltre l'automatizzazione della scrittura delle query SQL di Insert delle stesse entità, le query sono scritte in file .txt, pronte ad essere copiate ed incollate su Sql Server Management Studio ed essere eseguite

Facendo ciò il Database inizia ad essere popolato tramite i dati provenienti dal Dataset.
