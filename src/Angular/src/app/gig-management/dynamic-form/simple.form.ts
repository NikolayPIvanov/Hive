import {
    createMatConfig,
    DynMatRadioParams,
    DynMatSelectParams,
  } from '@myndpm/dyn-forms/ui-material';
  import { DynFormConfig } from '@myndpm/dyn-forms';
  
  export const simpleData = {
    billing: {
      firstName: 'Mynd',
      lastName: 'Management',
      address1: '1611 Telegraph Ave',
      address2: 'Suite 1200',
      country: 'US',
      zipCode: '94612',
    },
    account: 'GUEST',
    products: [
      {
        product: 'Product 1',
        quantity: 8,
      },
      {
        product: 'Product 2',
        quantity: 4,
      },
    ]
  };
  
  export const simpleForm: DynFormConfig<'edit'|'display'> = { // typed mode
    modeParams: {
      edit: { readonly: false },
      display: { readonly: true },
    },
    controls: [
      createMatConfig('ARRAY', {
        name: 'questions',
        factory: { cssClass: 'row' },
        params: {
          title: 'Questions',
          subtitle: 'Q&A Section',
          initItem: true,
        },
        controls: [
          createMatConfig('INPUT', {
            name: 'question',
            options: { validators: ['required'] },
            factory: { cssClass: 'col-6 col-md-8' },
            params: { label: 'Question *' },
          }),
          createMatConfig('INPUT', {
            name: 'answer',
            options: { validators: ['required', ['min', 1]] },
            factory: { cssClass: 'col-6 col-md-3 ml-5' },
            params: { label: 'Answer *', type: 'textarea' },
          }),
        ],
      }),
    ],
  };
  