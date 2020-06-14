<?php
class Visitor {
    //Database info
    private $connection;
    private $tablename = "visitors";

    //Model param
    public $ipAddress;
    public $system;
    public $browser;

    //Construct that connects with the db
    public function __construct($db) {
        $this->connection = $db;
    }

    public function add() {
        //Query one - downloading data to compare
        $query = "SELECT * FROM " . $this->tablename . " WHERE ipAddress =  '" . $this->ipAddress . "'";
        $getData = $this->connection->query($query);
             
            if(!is_array($getData->fetch())) {
                //Query two - insert new record to database
                $query = "INSERT INTO " . $this->tablename . " SET ipAddress=:ipAddress, system=:system, browser=:browser";
            
                $stmt = $this->connection->prepare($query);
    
                //Deleted html tags
                $this->ipAddress = htmlspecialchars(strip_tags($this->ipAddress));
                $this->system = htmlspecialchars(strip_tags($this->system));
                $this->browser = htmlspecialchars(strip_tags($this->browser));
    
                //Assign the value
                $stmt->bindParam(":ipAddress", $this->ipAddress);
                $stmt->bindParam(":system", $this->system);
                $stmt->bindParam(":browser", $this->browser);
    
                //Execute the query
                if($stmt->execute()) {
                    return true;
                }
                return false;
            }
            else {
                //Object already exist in database
                return true;
            }    
    }   
}
?>