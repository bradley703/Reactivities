import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useParams } from "react-router";
import { Link } from "react-router-dom";
import { Button, Card, Image } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/loadingComponent";
import { Activity } from "../../../app/models/activity";
import { useStore } from "../../../app/stores/store";


export default observer(function ActivityDetails(){
  const {activityStore} = useStore();
  const {selectedActivity: activity, loadActivity, loadingInitial} = activityStore;  
  const {id} = useParams<{id:string}>();
  console.log("NO id");
  useEffect(()=> {
    if(id) loadActivity(id);
  }, [id, loadActivity]);
  
  if(loadingInitial || !activity) return <LoadingComponent content = 'Loading activity'/>;
    return(
        <Card>
        <Image src= {`/assets/categoryImages/${activity.category}.jpg`}/>
        <Card.Content>
          <Card.Header>{activity.title}</Card.Header>
          <Card.Meta>
            <span>{activity.date}</span>
          </Card.Meta>
          <Card.Description>
            {activity.description}
          </Card.Description>
        </Card.Content>
        <Card.Content extra>
             <Button.Group width = '2'>
                 <Button  as={Link} to={`/manage/${activity.id}`} basic color = 'blue' content = 'Edit'/>
                 <Button  as={Link} to={'/activities'} basic color = 'blue' content = 'Cancel'/>

             </Button.Group>
        </Card.Content>
      </Card>  
    )
})