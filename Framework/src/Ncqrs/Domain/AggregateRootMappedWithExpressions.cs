﻿using System;
using System.Collections.Generic;
using Ncqrs.Domain.Mapping;
using Ncqrs.Eventing;

namespace Ncqrs.Domain
{
    /// <summary>
    /// A aggregate root that uses lambda style mapping to map internal event handlers. The following method should be mapped 
    /// </summary>
    /// <remarks>
    /// This aggregate root uses the  <see cref="ExpressionBasedEventDataHandlerMappingStrategy"/> to get the internal event handlers.
    /// </remarks>
    /// <seealso cref="ExpressionBasedEventDataHandlerMappingStrategy"/>
    public abstract class AggregateRootMappedWithExpressions : MappedAggregateRoot
    {
        [NonSerialized]
        private readonly IList<ExpressionHandler> _mappinghandlers = new List<ExpressionHandler>();

        /// <summary>
        /// Gets the <see cref="IList{ExpressionHandler}"/> list of mapping rules.
        /// </summary>
        internal IList<ExpressionHandler> MappingHandlers
        {
            get { return _mappinghandlers; }
        }

        protected AggregateRootMappedWithExpressions() 
            : base(new ExpressionBasedEventDataHandlerMappingStrategy())
        {
            /* I know, calling virtual methods from the constructor isn't the smartest thing to do
             * but in this case it doesn't really matter because the implemented 
             * method isn't (and shouldn't be) using any derived resources
            **/
            InitializeEventHandlers();
        }
        
        /// <summary>
        /// Maps the given generic eventtype to the expressed handler.
        /// </summary>
        /// <typeparam name="T">This should always be a <see cref="DomainEvent"/>.</typeparam>
        /// <returns>An <see cref="ExpressionHandler{T}"/>which allows us to define the mapping to a handler.</returns>
        protected ExpressionHandler<T> Map<T>() where T : IEvent<IEventData>
        {
            var handler = new ExpressionHandler<T>();
            _mappinghandlers.Add(handler);

            return handler;
        }

        ///<summary>
        /// Defines the method that derived types need to implement to support strongly typed mapping.
        ///</summary>
        public abstract void InitializeEventHandlers();
    }
}