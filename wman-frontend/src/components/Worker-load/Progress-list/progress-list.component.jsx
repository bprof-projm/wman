import React from 'react';
import ProgressCard from '../Progress-card/progress-card.component';

const ProgressList = (props) => {
    return (
        <div className="progress-card-list">
            {
                props.objects.map(object => 
                    //props needed: src, name, percent (workload)
                    <ProgressCard
                        key={props.object.name}
                        src={props.object.picture.src} 
                        name={props.object.name}
                        percent={props.object.workload}/>
                )}
        </div>
    )
}

export default ProgressList