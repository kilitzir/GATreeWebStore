library(httr)
library(jsonlite)
library(stringr)
library(dplyr)
library(crayon)

source("Utils.R")
#----------------------------- CREATE ATTRIBUTES
#This function creates a new table with attributes named clientDataAttributesTable
# and saved to globalvariables. 
GA.createAttributes <-function()
{
  my_data <- read.table(text = globalvariables1$dataAttributesText, stringsAsFactors = FALSE, sep = "\n" )
  
  linesOfAttributes = dim(my_data)[1] 
  capt = NULL  #Attributes's names
  for (i in 1:linesOfAttributes ) {
    txt = paste( my_data[i,])
    txt = gsub("\t"," ",txt) #Αφαίρεση τυχών αρχικών κενών ή tab
    if (trimws(substr(txt,1,1)) == "%") #just to be sure
      next 
    if (trimws(toupper(substr(txt,1,10)) == "@ATTRIBUTE")) 
    {
      capt = c(capt, substr(txt,12,nchar(txt)))
      next
    }
  }#for
  return( list(clientDataAttributesTable = capt))
}
#------------------------------ CREATE DATA SET
# Returns caption:
#     caption       colType                                     
#[1,] "sepallength" "NUMERIC"                                   
#[2,] "sepalwidth"  "NUMERIC"                                   
#[3,] "petallength" "NUMERIC"                                   
#[4,] "petalwidth"  "NUMERIC"                                   
#[5,] "class"       "Iris-setosa;Iris-versicolor;Iris-virginica"

# class : which attribute was used as class:
#$class[[1]]
#[1] "class"
GA.createDataSet <- function() 
{ 
  globalvariables1$dataset[globalvariables1$dataset == "?"] <- NA #Αντικατάσταση "?" με ΝΑ
  globalvariables1$dataset[globalvariables1$dataset == "?"] <- "" #????
  
  caption = NULL #Πίνακας  με 2 λίστες, η 1η έχει το όνομα τού χαρακτηριστικού 
  # και το 2ο με τις τιμές της
  #Η πρώτη λέξη της λίστας capt έχει τα ονόματα των χαρακτηριστικών
  caption = unlist(globalvariables1$dataAttributesTable["name"],use.names = FALSE)
  capt = NULL  #Όνομα και τύπος χαρακτηριστικών  
  #Διάβασμα των τύπων δεδομένων
  capt = as.vector(globalvariables1$dataAttributesTable[,"type"] )
  colType =  NULL
  colType= vector(mode="list", length=length(capt)) 
  for (j in 1:length(capt))
  { 
    if (toupper(capt[j]) == "STRING")
    {
      temp <- levels(as.factor(globalvariables1$dataset[,j]))
      if (length(temp) > 100)
      {
        caption[j] <- paste(caption[j], "Too many diferent values. Do Not Use")
        colType[j] = "Many Values"
        next
      } 
      temp <- temp[!(temp)] 
      colType[[j]]=temp
      next
    }
    if (toupper(capt[j]) == "NUMERIC" ) 
    {
      colType[j]="NUMERIC"
      next
    }
    else  # Nominal values
    {
      browser()
      colType[j] = strsplit(globalvariables1$dataAttributesTable[j,"districtValues"],",") #λίστα διακριτών τιμών         
    }
  } # for
  caption = cbind(caption, colType)
  #TODO: Remove class!!
  #Επιλογή Χαρακτηριστικών και Κλάσης
  cat("\nPlease select Attribute(s):")
  cCaption= select.list(caption[,1] , multiple = TRUE ) # list with the characteristics's names
  
  #Επιλογή Κλάσης
  caption = caption[caption[,1] %in% c(cCaption),]
  cat("\nPlease select the Class Attribute \n")
  cClass= select.list(cCaption)
  Data <- list(caption=caption, class=cClass)
  return(Data)
}
#----------------------------    CREATE ONE TREE --------------------------
GA.create_one_tree <- function(V, DataTypes, data_min,data_max){
  #Παράμετροι:
  #V: (Αριθμός) Χαρακτηριστικών 
  #DataTypes: Οι τύποι δεδομένων κάθε χαρακτηριστικού ("NUMERIC" ή λίστα διακριτών τιμών)
  #data_min: ελάχιστο δεδομένων.
  #data_max: μέγιστο δεδομένων.
  # returns:
  # A tree with two children(leaves) (matrix) with columns :
  #       id:  vector 1:3 i.e. [1 2 3] - That is how y create a tree using a table
  #       var: vector[3]:
  #                     [1]: random datatype (int)
  #                     [2]: 0
  #                     [3]: 0
  #             The datatype is being used for going left or right. In children is empty
  #       split: vector[3]:
  #               [1]: (random number between high-low or random discrete value) of the var[1] datatype
  #               [2]: 0
  #               [3]: 0
  #         The split that decides whether you go left or right. In children is empty
  #       class: vector[3] - class that belongs to.
  #               [1] : 0
  #              [2:3]: random datatype (int)
  #        If empty, it has child, if not empty, it's a leaf.
  #       lchild: c(2,0,0) 
  #       rchild: c(3,0,0)  
  #       rownames: id, δηλ 1,2,3 (why?)
  # for example a tree:
  #   id  var   split    class lChild rChild
  #1  1   2   3.898747     0      2      3
  #2  2   0   0.000000     3      0      0
  #3  3   0   0.000000     1      0      0
  
  #μεταβλητή διαχωρισμού (κόμβοι: θα έχουν τιμή, φύλλα: var = 0).
  var <- vector(mode="numeric", length=3) 
  #τιμή διαχωρισμού (κόμβοι: θα έχουν τιμή, φύλλα: split = 0).
  split <- vector(mode="numeric", length=3)
  #class output (φύλλα: θα έχουν τιμή, κόμβοι: class = 0). CK: better name !!!!!!
  class <- vector(mode="numeric", length=3)
  
  #επιλογή ενός τυχαίου χαρακτηριστικού από 1 ως V. Η τιμή ανατίθεται μόνο για τη ρίζα (κόμβος 1).
  var[1] <- sample(1:V, 1)
  cat(crayon::bgCyan(var[1]))
  if (DataTypes[[var[1]]][1] == "NUMERIC") {
    #τυχαία τιμή μεταξύ τoυ ελάχιστου και του μέγιστου των δεδομένων. Η τιμή ανατίθεται μόνο για τη ρίζα (κόμβος 1).
    minV <- as.numeric(gsub(",",".",data_min[var[1]])) # comma for greek, decimal point for others
    maxV <- as.numeric(gsub(",",".",data_max[var[1]]))
    split[1] = runif(1, minV, maxV)
  } 
  else 
  { # Το χαρακτηριστικό παίρνει διακριτές τιμές 
    #Τυχαία τιμή από τις διακριτές της μεταβλητης
    # SOS - fix it!!
    split[1] = trim.leading(sample(DataTypes[[var[1]]],1))
  }
  #επιλογή μιας τυχαίας κλάσης από 1 ως Nc. Η τιμή ανατίθεται μόνο για τα φύλλα (κόμβοι 2 και 3).
  class[2:3] = sample(1:Nc, 2, replace=T) # CK Why not exclude the classification class?
  #set children. Μόνο ο 1ος κόμβος έχει παιδιά.
  lChild = c(2,0,0) 
  rChild = c(3,0,0)
  #ids. αριστερό παιδί είναι 2*κ, δεξί παιδί είναι 2*κ+1.
  id = 1:3
  tree <- cbind(id, var, split, class, lChild, rChild)
  row.names(tree) = as.character(id) # CK depreciated rownames
  return(tree)
}
#-------------------------------------- CONVERT TO TREE
#converts a string to tree
# tree: vector of strings.
# returns a tree like the following:
# id  var    split     class lChild rChild
#  1   4     2.375548     0      2      3
#  2   0     0.000000     2      0      0
#  3   0     0.000000     3      0      0
GA.convert_A_tree_to_table <- function(Ltree)
{
  #browser()
  tree<- unlist(strsplit(Ltree,","))
  num_rows <- length(tree) %/% 6
  
  my_data_frame <- data.frame(col1 = numeric(num_rows), col2 = numeric(num_rows), col3 = numeric(num_rows), 
                              col4 = numeric(num_rows), col5 = numeric(num_rows), col6 = numeric(num_rows))
  
  my_data_frame$col1 <- tree[1:num_rows]
  my_data_frame$col2 <- tree[(num_rows + 1):(num_rows * 2)]
  my_data_frame$col3 <- tree[((num_rows * 2)+1):(num_rows * 3)]
  my_data_frame$col4 <- tree[((num_rows * 3)+1):(num_rows * 4)]
  my_data_frame$col5 <- tree[((num_rows * 4)+1):(num_rows * 5)]
  my_data_frame$col6 <- tree[((num_rows * 5)+1):(num_rows * 6)]
  names(my_data_frame) <- globalvariables1$tree_labels
  return (my_data_frame)
}
#-------------------------------------- PAYOFF TREE
GA.payoff <- function(tree, Data, DataTypes, Class, x){
  #tree: the tree we evaluate
  #Data: Ο πίνακας με τα δεδομένα χωρίς την στήλη τής κλάσης
  #DataTypes: Οι τύποι δεδομένων κάθε χαρακτηριστικού ("NUMERIC" ή λίστα διακριτών τιμών)
  #Class: Οι πραγματικές κλάσεις των δεδομένων - ι.ε. η στήλη με την τιμή της κλάσης
  #x: Συντελεστής διόρθωσης (?)
  # returns: (number:) the payoff
  # CK 1. Find, for that tree, for all the data, the prediction values
  
  p = GA.predict_tree(tree, Data, DataTypes)    #πρόβλεψη με χρήση της predict_tree.
  # CK 2. c: the actual values  
  cc = as.numeric(Class)                      #διάνυσμα με τις κλάσεις.
  # CK 3. Find the correct values
  correctClassified = sum(cc == p)            #αριθμός σωστών δειγμάτων.
  # CK 4. evaluate
  L = length(tree[,"id"])                    #αριθμός κόμβων.
  a = (correctClassified^2) * x / (L^2 + x) 
  return(a)
}
#----------------------------------    PREDICT TREE
GA.predict_tree <- function(tree, Data, DataTypes){
  #Παράμετροι:
  #tree: Το δέντρο με βάση το οποίο κάνουμε πρόβλεψη
  #Data: Ο πίνακας με τα δεδομένα χωρίς την στήλη τής κλάσης
  #DataTypes: Οι τύποι δεδομένων κάθε χαρακτηριστικού ("NUMERIC" ή λίστα διακριτών τιμών)
  # returns: a vector with the prediction on data for this particular tree
  # sample tree:
  #id  var    split     class lChild rChild
  #1   4     2.375548     0      2      3
  #2   0     0.000000     2      0      0
  #3   0     0.000000     3      0      0
  
  #browser()
  N <- dim(Data)[1]                             #αριθμός δειγμάτων δεδομένων.
  prediction = vector(mode="numeric", length=N) 
  prediction = GA.get_prediction(); #cut throat
  
  for(d in 1:N){  #Για κάθε δείγμα
    k <- "1"                                    #ξεκινάμε από την ρίζα.
    while(!is.na(tree[k,"var"]) && tree[k,"var"]!= 0 ){
      #while( tree[k,"var"]!= 0){               #Αν var == 0, τότε είμαστε σε φύλλο.
      v = tree[k,"var"]
      v=as.integer(v)
      if (length(Data[d,v]) == 0)
      {
        next
      }
      if (is.na(Data[d,v]))  # εάν έχουμε missing value
        #επιλέγεται τυχαία ένα από τα δύο παιδιά
        k<- sample(c(as.character(tree[k,"lChild"]),as.character(tree[k,"rChild"])),1)
      else 
        if (DataTypes[[v]][1] == "NUMERIC" ) {  
          if (Data[d,v] < tree[k,"split"]){         #Αν var < split
            #αριστερά
            k <- as.character(tree[k,"lChild"])
          }else{
            #δεξιά
            k <- as.character(tree[k,"rChild"])
          }
        } else {  # Το χαρακτηριστικό παίρνει διακριτές τιμές 
          if (Data[d,v] == tree[k,"split"]){         #Αν var = split
            #αριστερά
            k <- as.character(tree[k,"lChild"])
          }else{
            #δεξιά
            k <- as.character(tree[k,"rChild"])
          }      
        } # else {  # Το χαρακτηριστικό παίρνει διακριτές τιμές
    }# while((v=tree[k,"var"]) > 0)
    
    prediction[d] <- tree[k,"class"]       #η κλάση του φύλλου k.
  }#for(d in 1:N)
  prediction = GA.get_prediction(); #TODO: fix itcut throat 
  return(prediction)
}
#------------------------------------- CROSS OVER
GA.crossover <- function(parent1,parent2){
  
  #L1, L2: non-leaves nodes.
  L1 = length(parent1[,"id"]) 
  L2 = length(parent2[,"id"])
  #τυχαία επιλογή 1 κόμβου για κάθε γονέα.
  i1 = sample(1:L1,1)
  i2 = sample(1:L2,1)
  #id κόμβου για κάθε γονέα.
  node1 = parent1[i1,"id"] 
  node2 = parent2[i2,"id"]
  child1 = parent1 
  child2 = parent2
  
  #ανταλλαγή υποδέντρων με χρήση της copy_subtree.
  child1 = GA.copy_subtree(parent2, node2, child1, node1)
  child2 = GA.copy_subtree(parent1, node1, child2, node2)  
  return (list(tree1=child1, tree2=child2))
  return(trees)
}
#--------- COPY SUBTREE
GA.copy_subtree <- function(source, source_node, target, target_node){
  #Παράμετροι:
  #source: Το δέντρο από το οποίο θα αντιγράψουμε υποδέντρο
  #source_node: ο αριθμός κόμβου του source απο τον οποίο θα γίνει η αντιγραφή
  #target: Το δέντρο στο το οποίο θα αντιγράψουμε υποδέντρο
  #target_node: ο αριθμός κόμβου του target στον οποίο θα γίνει η αντιγραφή
  
  #διαγραφή όλων των κόμβων που βρίσκονται κάτω απο τον target κόμβο. 
  old_nodes = GA.get_subtree(target, target_node)
  target[old_nodes,"id"] = -1
  to_keep =(as.integer(target[,"id"]) > 0)
  target = target[to_keep,]
  
  #προσθέτει νέους κόμβους, ενημερώνοντας id, rChild, lChild ανάλογα με τη θέση του κόμβου στόχου.
  source_subtree = GA.get_subtree(source, source_node) 
  new_ids = c(target_node) #vector with nodes to be copied
  for(i in 1:length(source_subtree)){
    #copy τις πληροφορίες στο new_node.
    new_node = source[source_subtree[i],]
    #set το νέο id.
    new_node["id"] = new_ids[1]
    if (new_node["var"] > 0){
      #εάν είναι εσωτερικός κόμβος ενημερώνονται τα παιδιά. 
      new_node["lChild"] = 2*as.integer(new_ids[1])
      new_node["rChild"] = 2*as.integer(new_ids[1]) + 1
      new_ids = c(new_ids, 2*as.integer(new_ids[1]), 2*as.integer(new_ids[1]) + 1) 
    }else{
      #φύλλο.
      new_node["lChild"] = 0
      new_node["rChild"] = 0
    }
    #διαγραφή κόμβου από new_ids.
    new_ids = new_ids[-1]
    #προσθέτει νέα γραμμή στο δέντρο (τον κόμβο). 
    target = rbind(target, new_node)
  }
  #row.names(target) = as.character(target[,"id"])# CK Why it's needed?
  return(target)
}
#-------- GET SUBTREE
GA.get_subtree <- function(tree, i){
  #H get subtree παίρνει ένα υποδέντρο ξεκινώντας από έναν κόμβο i ενός δέντρου με N κόμβους.
  #Παράμετροι:
  #tree: Το αρχικό δέντρο 
  #i: Αριθμός κόμβου από το  οποίο θα πάρουμε το υποδέντρο
  
  subtree = c() 
  to_check = c(i) # list with nodes to be (pushed) and checked
  
  while(length(to_check)>0){
    n <- -1
    #n = as.character(to_check[1])
    n = as.numeric(to_check[1])
    subtree = c(subtree, n)   #εισάγει τον κόμβο στο υποδέντρο. 
    to_check = to_check[-1]   #διαγραφή του κόμβου από τη λίστα.
    
    if (length(tree[n,"var"]) == 0 || is.na(tree[n,"var"])){
      next
    }
    if (as.numeric(tree[n,"var"])!=0){       #αν είναι εσωτερικός κόμβος τότε εισάγονται τα παιδιά του στη λίστα.
      to_check = c(to_check, tree[n,"lChild"], tree[n,"rChild"])
    }
  }
  return(subtree)
}
#------------------------------------------ SAMPLE prediction
GA.get_prediction <- function()
{
  min_val <- 1
  max_val <- max(globalvariables1$dataset$classLevel)
  possible_values <- min_val:max_val
  prediction = sample(possible_values, size = length(globalvariables1$dataset$classLevel),replace = TRUE)
  return (prediction)
}
#------------------------------------------ MUTATE
#------------------- mutate --------------------------------
GA.mutate <-function(tree, DataTypes, data_min, data_max, Nc){
  #μετάλλαξη των δέντρων με χρήση της p_mutate.
  #Παράμετροι:
  #trees: λίστα με τα δέντρα της γεννιάς 
  #DataTypes: Οι τύποι δεδομένων κάθε χαρακτηρηστικού ("NUMERIC" ή λίστα διακριτών τιμών)
  #data_min: Πίνακες με την ελάχιστη τιμή του κάθε NUMERIC χαρακτηριστικού 
  #data_max: Πίνακες με την μέγιστη τιμή του κάθε NUMERIC χαρακτηριστικού 
  #Nc: Πλήθος διακριτών τιμών κλάσης
  #p_mutate: πιθανότητα μετάλλαξης
  browser()
      L = length(tree[,"id"])  #μήκος δέντρου.
      n = sample(1:L, 1)       #επιλογή ενός τυχαίου κόμβου.
      #αν δεν είναι φύλλο ο επιλεγμένος κόμβος.
      if (tree[n,"class"] == 0) {
        #τότε ο κόμβος παίρνει var την τιμή του κόμβου n.
        var = as.integer(tree[n,"var"])
        
        #και τιμή διαχωρισμού μια τυχαία μεταξύ του πάνω και του κάτω ορίου.
        
        #tree[n,"split"] <- runif(1, data_min[var], data_max[var])
        if (DataTypes[[as.integer(var[1])]][1] == "NUMERIC") {
          tree[n,"split"] = runif(1, as.double(data_min[var]), as.double(data_max[var]))
        } 
        else {
          tree[n,"split"] = trim.leading(sample(DataTypes[[var]],1))
        }
        ######################################################********************************************************
      } else {               #αν είναι φύλλο.
        #παίρνει μια από τις κλάσεις τυχαία.
        tree[n,"class"] <- sample(1:Nc, 1)
      }
  return(tree) 
}
