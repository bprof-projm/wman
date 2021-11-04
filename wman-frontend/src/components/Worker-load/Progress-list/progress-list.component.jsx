import React from 'react';
import ProgressCard from '../Progress-card/progress-card.component';
import './progress-list.styles.css'
//props needed objects(list)
const ProgressList = (props) => {
    return (
        <div className="progress-card-list">
            {
                props.objects.map(object => 
                    //props needed: src, name, percent (workload)
                    <ProgressCard
                        key={object.name}
                        src={object.picture.src} 
                        name={object.name}
                        percent={object.workload}/>
                )}
        </div>
    )
}

export default ProgressList