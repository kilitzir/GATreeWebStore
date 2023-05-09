readFile <- function(filename = NULL) { 
  #ΠΑΡΑΜΕΤΡΟΣ: Όνομα αρχείου 
  #Εάν δεν περαστεί όνομα αρχείου σαν παράμετρος τότε ανοίγει 
  #πλαίσιο διαλόγου επιλογής αρχείου
  #returns: a list with
  # $table_data : ALL the data read
  # $caption : table with attributes and their type of value
  # $class :  a string with the classification attribute
  if (is.null(filename)) {
    Filters <- matrix(c("Arff files", "*.arff", "All files", "*"),
                      2, 2, byrow = TRUE)
    filename <- choose.files(default = "*.arff", caption = "Select file",
                             multi = FALSE, filters = Filters,
                             index = nrow(Filters))
  }
  #Διάβασμα αρχείου σε γραμμές
  my_data <- read.delim(filename, header = FALSE,sep = "\n", stringsAsFactor = FALSE)
  
  linesOFfile = dim(my_data)[1] #Πλήθος γραμμών αρχείου
  capt = NULL  #Όνομα και τύπος χαρακτηριστικών  
  
  for (i in 1:linesOFfile ) {
    
    txt = paste( my_data[i,])
    txt = gsub("\t"," ",txt) #Αφαίρεση τυχών αρχικών κενών ή tab
    if (trim.leading(substr(txt,1,1)) == "%") next #Γραμμή με σχόλια
    else if (trim.leading(toupper(substr(txt,1,10)) == "@ATTRIBUTE")){
      capt=c(capt, substr(txt,12,nchar(txt))) # προσθέτουμε χαρακτηριστικά
    } else if (trim.leading(toupper(substr(txt,1,5))== "@DATA")){
      #εάν διαβάσουμε @DATA οι επόμενες γραμμές έχουν δεδομένα
      
      if (is.null(capt))  {
        cat("ERROR NO ATTRIBUTES.\n")
      } else { 
        my_data <- my_data[i+1:linesOFfile,] #διαβάζουμε τα δεδομένα
        my_data <- na.omit(my_data)    #Αφαίρεση τυχών κενών γραμμών
        my_data <- my_data[!grepl("%",my_data, useBytes = TRUE)] #αφαίρεση γραμμών σχολίων
        #μετατροπή σε πίνακα
        table_data <- read.csv(text = paste(my_data, collapse='\n'), 
                               header = FALSE, 
                               stringsAsFactors = FALSE, 
                               sep = ',')
        table_data[table_data == "?"] <- NA #Αντικατάσταση "?" με ΝΑ
        table_data[table_data == "'"] <- "" 
        
        #διαγραφή αρχικών κενών
        # for (j in 1:length(table_data[,1])){
        #  table_data[j,] = lapply(table_data[j,], trim.leading)ξ
        #}
        
        caption = NULL #Πίνακας  με 2 λίστες, η 1η έχει το όνομα τού χαρακτηριστικού και το 2ο με τις τιμές της
        #Η πρώτη λέξη της λίστας capt έχει τα ονόματα των χαρακτηριστικών
        caption = c(caption, substr(trim.leading(capt),1,unlist(lapply(gregexpr(pattern = ' ', trim.leading(capt)), min))-1))
        #διαγράφουμε τα κενά πριν και μετά το ονόματα χαρακτηριστικών
        capt = trimws(capt, whitespace = "[ \t\r\n]")
        #capt=trim.leading(capt)
        #Διάβασμα των τύπων δεδομένων
        capt = substr(capt,unlist(lapply(gregexpr(pattern = ' ', capt), min))+1, nchar(capt))
        capt = trim.leading(capt) 
        colType= vector(mode="list", length=length(capt)) 
        
        for (j in 1:length(capt)){ # CK better use seq_len
          if (toupper(capt[j]) == "STRING"){
            
            temp <- levels(as.factor(table_data[,j]))
            if (length(temp) > 100) {
              caption[j] <- paste(caption[j], "Too many diferent values. Do Not Use")
              colType[j] = "Many Values"
            } else {
              temp <- temp[!(temp)] 
              colType[[j]]=temp
            }  
          }
          else if (toupper(capt[j]) == "NUMERIC" || (toupper(capt[j]) == "REAL")|| (toupper(capt[j]) == "INTEGER")) 
          {
            
            colType[j]="NUMERIC"
          }
          else 
          {
            
            colType[j]=strsplit(substr(capt[j],2,nchar(capt[j])-1),",") #λίστα διακριτών τιμών         
          }
        } # for
        caption = cbind(caption, colType)
        
        if (ncol(table_data) == length(caption[,1])) colnames(table_data) <- caption[,1]
        else cat("ERROR Num of columns and num of attr are diferent \n") 
        break
      }
    }
  }
  #Επιλογή Χαρακτηριστικών και Κλάσης
  cat("\nPlease select Attribute(s) and Class columns \n")
  cCaption= select.list(caption[,1] , multiple = TRUE ) # list with the characteristics's names
  
  #Επιλογή Κλάσης
  caption = caption[caption[,1] %in% c(cCaption),]
  cat("\nPlease select the Class Attribute \n")
  cClass= select.list(cCaption)
  Data <- list(table_data=table_data,caption=caption, class=cClass)
  return(Data)
}
